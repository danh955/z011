// <copyright file="StockSymbolUpdaterHandler.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Application.StockSymbolUpdater
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.NasdaqSymbols;

    /// <summary>
    /// Command handler for updating the database symbols.
    /// </summary>
    public class StockSymbolUpdaterHandler : AsyncRequestHandler<StockSymbolUpdaterCommand>
    {
        private readonly IDbContextFactory<EntityDbContext> contextFactory;
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockSymbolUpdaterHandler"/> class.
        /// </summary>
        /// <param name="mediator">IMediator.</param>
        /// <param name="contextFactory">IDbContextFactory for AppEntityDbContext.</param>
        public StockSymbolUpdaterHandler(IMediator mediator, IDbContextFactory<EntityDbContext> contextFactory)
        {
            this.mediator = mediator;
            this.contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        protected override async Task Handle(StockSymbolUpdaterCommand request, CancellationToken cancellationToken)
        {
            using var db = this.contextFactory.CreateDbContext();

            var allSymbolsTask = this.mediator.Send(new NasdaqSymbolsQuery(), cancellationToken);
            var stocksTask = db.Stocks.AsNoTracking().Select(s => s.Symbol).ToListAsync(cancellationToken);
            await Task.WhenAll(allSymbolsTask, stocksTask);

            var dbSymbols = stocksTask.Result.ToHashSet();
            var allSymbols = allSymbolsTask.Result.NasdaqSymbols
                    .Where(s => s.Symbol.All(char.IsLetter) && !s.TestIssue && s.FinancialStatus == 'N')
                    .Select(s => new { Symbol = s.Symbol.ToUpper(), Name = s.SecurityName })
                .Concat(allSymbolsTask.Result.OtherSymbols
                    .Where(s => s.NASDAQSymbol.All(char.IsLetter) && !s.TestIssue)
                    .Select(s => new { Symbol = s.NASDAQSymbol.ToUpper(), Name = s.SecurityName }))
                .ToList();

            var newSymbols = allSymbols
                .Where(s => !dbSymbols.Contains(s.Symbol))
                .Select(s => new StockEntity() { Symbol = s.Symbol, Name = s.Name })
                .ToList();

            if (newSymbols.Any())
            {
                db.Stocks.AddRange(newSymbols);
                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}