// <copyright file="StockChangeGrid.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Z011.Domain.Entities;

    /// <summary>
    /// Stock change grid class.
    /// This will get a list of stock and each column will be each years percentage change.
    /// </summary>
    public static class StockChangeGrid
    {
        /// <summary>
        /// Get nothing query.
        /// </summary>
        public class Query : IRequest<Result>
        {
        }

        /// <summary>
        /// Stock change grid result.
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Gets headings for the percentages.
            /// </summary>
            public IEnumerable<DateTime> Headings { get; internal set; }

            /// <summary>
            /// Gets list of rows.
            /// </summary>
            public IEnumerable<Row> Rows { get; internal set; }
        }

        /// <summary>
        /// Each row of symbol and percentages class.
        /// </summary>
        public class Row
        {
            /// <summary>
            /// Gets stock symbol for row.
            /// </summary>
            public string Symbol { get; internal set; }

            /// <summary>
            /// Gets a list of percentages for the row.
            /// </summary>
            public IEnumerable<double?> Percentages { get; internal set; }
        }

        /// <summary>
        /// Handler class.
        /// </summary>
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IDbContextFactory<EntityDbContext> contextFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="contextFactory">IDbContextFactory.</param>
            public Handler(IDbContextFactory<EntityDbContext> contextFactory)
            {
                this.contextFactory = contextFactory;
            }

            /// <inheritdoc/>
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                using var db = this.contextFactory.CreateDbContext();

                var prices = await db.StockPrices
                                .Where(p => p.Frequency == Frequency.Yearly)
                                .Select(p => new { p.Stock.Symbol, p.Period, Close = p.AdjClose, Open = (((p.AdjClose - p.Close) / p.Close) + 1) * p.Open })
                                .OrderBy(p => p.Symbol).ThenBy(p => p.Period)
                                .ToListAsync(cancellationToken);

                var headings = prices.Select(p => p.Period).Distinct().OrderBy(p => p).ToList();
                var headingIndex = new Dictionary<DateTime, int>(headings.Select((d, number) => new KeyValuePair<DateTime, int>(d, number++)));

                var rows = new List<Row>();
                string symbol = string.Empty;
                double open = 0;
                double?[] percentages = null;

                foreach (var price in prices)
                {
                    if (price.Symbol != symbol)
                    {
                        symbol = price.Symbol;
                        open = price.Open;
                        percentages = new double?[headings.Count];
                        rows.Add(new Row
                        {
                            Symbol = price.Symbol,
                            Percentages = percentages,
                        });
                    }

                    percentages[headingIndex[price.Period]] = (price.Close - open) / open;
                    open = price.Close;
                }

                return new Result() { Headings = headings, Rows = rows };
            }
        }
    }
}