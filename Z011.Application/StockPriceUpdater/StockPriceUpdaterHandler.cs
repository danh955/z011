// <copyright file="StockPriceUpdaterHandler.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Application.StockPriceUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.YahooPriceHistory;

    /// <summary>
    /// Command handler for updating the database stock prices.
    /// </summary>
    public class StockPriceUpdaterHandler : AsyncRequestHandler<StockPriceUpdaterCommand>
    {
        private readonly ILogger<StockPriceUpdaterHandler> logger;
        private readonly IDbContextFactory<EntityDbContext> contextFactory;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockPriceUpdaterHandler"/> class.
        /// </summary>
        /// <param name="mediator">IMediator.</param>
        /// <param name="logger">ILogger.</param>
        /// <param name="contextFactory">IDbContextFactory for AppEntityDbContext.</param>
        public StockPriceUpdaterHandler(IMediator mediator, ILogger<StockPriceUpdaterHandler> logger, IDbContextFactory<EntityDbContext> contextFactory)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        protected override async Task Handle(StockPriceUpdaterCommand request, CancellationToken cancellationToken)
        {
            YahooInterval interval = request.Frequency switch
            {
                Frequency.Daily => YahooInterval.Daily,
                Frequency.Weekly => YahooInterval.Weekly,
                Frequency.Monthly => YahooInterval.Monthly,
                Frequency.Quarterly => YahooInterval.Quorterly,
                _ => throw new NotImplementedException(),
            };

            using var db = this.contextFactory.CreateDbContext();

            // Get list of symbols to process.
            var stocks = await db.Stocks.AsNoTracking()
                .Select(s => new { s.Id, s.Symbol, })
                .ToListAsync(cancellationToken);

            int count = 20;
            foreach (var item in stocks)
            {
                if (count-- <= 0)
                {
                    return;
                }

                // Get the database stock price.
                var oldPrices = await db.StockPrices
                    .Where(p => p.StockId == item.Id && p.Frequency == request.Frequency)
                    .OrderBy(p => p.Period)
                    .ToListAsync(cancellationToken: cancellationToken);

                DateTime? firstDate = oldPrices.Select(p => (DateTime?)p.Period).FirstOrDefault();
                DateTime? lastDate = oldPrices.Select(p => (DateTime?)p.Period).LastOrDefault();

                var query = new YahooPriceHistoryQuery
                {
                    Symbol = item.Symbol,
                    FirstDate = firstDate ?? DateTime.Today.AddYears(-10),
                    LastDate = DateTime.Today,
                    Interval = interval,
                };

                // Get prices.
                var yahooResult = await this.mediator.Send(query, cancellationToken);

                if (yahooResult.ErrorMessage != null)
                {
                    this.logger.LogWarning("{0} for {1} from {2:M-d-Y} to {3:M-d-Y} for {4}", yahooResult.ErrorMessage, query.Symbol, query.FirstDate, query.LastDate, query.Interval);
                }
                else
                {
                    // Update prices if any.
                    if (oldPrices.Any())
                    {
                        List<StockPriceEntity> toUpdate = new();
                        var lookup = new Dictionary<DateTime, YahooPrice>(yahooResult.Prices.Select(a => new KeyValuePair<DateTime, YahooPrice>(a.Date, a)));
                        foreach (var oldItem in oldPrices)
                        {
                            if (lookup.TryGetValue(oldItem.Period, out YahooPrice quote) &&
                                (oldItem.Open != quote.Open
                                || oldItem.High != quote.High
                                || oldItem.Low != quote.Low
                                || oldItem.Close != quote.Close
                                || oldItem.AdjClose != quote.AdjClose
                                || oldItem.Volume != quote.Volume))
                            {
                                oldItem.Open = quote.Open;
                                oldItem.High = quote.High;
                                oldItem.Low = quote.Low;
                                oldItem.Close = quote.Close;
                                oldItem.AdjClose = quote.AdjClose;
                                oldItem.Volume = quote.Volume;
                                toUpdate.Add(oldItem);
                            }
                        }

                        if (toUpdate.Any())
                        {
                            db.StockPrices.UpdateRange(toUpdate);
                            await db.SaveChangesAsync(cancellationToken);
                        }
                    }

                    // Add prices if any.
                    var toAdd = yahooResult.Prices.Where(q => lastDate == null || q.Date > lastDate)
                        .Union(yahooResult.Prices.Where(q => lastDate != null && q.Date < firstDate))
                        .Select(q => new StockPriceEntity()
                        {
                            StockId = item.Id,
                            Frequency = request.Frequency,
                            Period = q.Date,
                            Open = q.Open,
                            High = q.High,
                            Low = q.Low,
                            Close = q.Close,
                            AdjClose = q.AdjClose,
                            Volume = q.Volume,
                        });

                    if (toAdd.Any())
                    {
                        db.AddRange(toAdd);
                        await db.SaveChangesAsync(cancellationToken);
                    }
                }

                await Task.Delay(100, cancellationToken);
            }
        }
    }

    //// https://query1.finance.yahoo.com/v7/finance/download/{ticker}?period1={start_time}&period2={end_time}&interval={interval}&events=history
    //// https://query1.finance.yahoo.com/v7/finance/download/msft?period1=1609459200&period2=1615429161&interval=1d&events=history
    //// https://stackoverflow.com/questions/2883576/how-do-you-convert-epoch-time-in-c
}