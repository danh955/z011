// <copyright file="StockSymbolUpdaterTest.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ApplicationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;
    using Z011.Application.StockSymbolUpdater;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.NasdaqSymbols;
    using Z011.TestHelper;

    /// <summary>
    /// Test stock symbol updater class.
    /// </summary>
    public class StockSymbolUpdaterTest
    {
        /// <summary>
        /// Handler test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task StockSymbolUpdaterCommandHandlerTest()
        {
            using var contextFactory = new InMemoryDbContextFactorySQLite();
            var mockResult = GetMockResult();

            using (var db = contextFactory.CreateDbContext())
            {
                // Load the database with one stock.
                var stock = mockResult.NasdaqSymbols.First();
                db.Stocks.Add(new StockEntity() { Symbol = stock.Symbol, Name = stock.SecurityName });
                await db.SaveChangesAsync();
            }

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<NasdaqSymbolsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockResult);

            var command = new StockSymbolUpdaterCommand();
            var handler = new StockSymbolUpdaterHandler(mediatorMock.Object, contextFactory);
            await ((IRequestHandler<StockSymbolUpdaterCommand>)handler).Handle(command, CancellationToken.None);

            List<StockEntity> dbStocks;
            using (var db = contextFactory.CreateDbContext())
            {
                dbStocks = await db.Stocks.ToListAsync();
            }

            Assert.Equal(6, dbStocks.Count);
        }

        private static NasdaqSymbolsResult GetMockResult()
        {
            var nasdaqSymbols = new List<NasdaqSymbolDto>()
            {
                new NasdaqSymbolDto("AACG", "ATA Creativity Global", 'G', false, 'N', 100, false, false),
                new NasdaqSymbolDto("CASY", "Casey's General Stores, Inc.", 'Q', false, 'N', 100, false, false),
                new NasdaqSymbolDto("ESPO", "VanEck Vectors Video Gaming and eSports ETF", 'G', false, 'N', 100, true, false),
            };

            var otherSymbol = new List<OtherSymbolDto>()
            {
                new OtherSymbolDto("A", "Agilest Technologies", 'N', "A", false, 100, false, "A"),
                new OtherSymbolDto("ECL", "Ecolab Inc. Common Stock", 'N', "ECL", false, 100, false, "ECL"),
                new OtherSymbolDto("HE", "Hawaiian Electric Industries, Inc. Common Stock", 'N', "HE", false, 100, false, "HE"),
            };

            return new NasdaqSymbolsResult(nasdaqSymbols, new DateTime(2021, 1, 21), otherSymbol, new DateTime(2021, 1, 21));
        }
    }
}