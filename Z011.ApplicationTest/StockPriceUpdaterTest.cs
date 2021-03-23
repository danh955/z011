// <copyright file="StockPriceUpdaterTest.cs" company="None">
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
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Z011.Application.StockPriceUpdater;
    using Z011.Domain.Entities;
    using Z011.Infrastructure.YahooPriceHistory;
    using Z011.TestHelper;

    /// <summary>
    /// Test stock price updater class.
    /// </summary>
    public class StockPriceUpdaterTest
    {
        /// <summary>
        /// Handler test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task StockPriceUpdaterHandlerTest()
        {
            using var contextFactory = new InMemoryDbContextFactorySQLite();
            using (var db = contextFactory.CreateDbContext())
            {
                db.Stocks.Add(new StockEntity() { Symbol = "MSFT", Name = "Microsoft" });
                await db.SaveChangesAsync();
            }

            ILogger<StockPriceUpdaterHandler> logger = Mock.Of<ILogger<StockPriceUpdaterHandler>>();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<YahooPriceHistoryQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(GetMockResult());

            var command = new StockPriceUpdaterCommand() { Frequency = Frequency.Monthly };
            var handler = new StockPriceUpdaterHandler(mediatorMock.Object, logger, contextFactory);
            await ((IRequestHandler<StockPriceUpdaterCommand>)handler).Handle(command, CancellationToken.None);

            using (var db = contextFactory.CreateDbContext())
            {
                var prices = await db.StockPrices.Select(p => p).ToListAsync();
                Assert.Equal(7, prices.Count);
            }
        }

        private static YahooResult GetMockResult()
        {
            List<YahooPrice> prices = new()
            {
                new YahooPrice(new DateTime(2020, 10, 01), 213.490005, 225.210007, 199.619995, 202.470001, 201.477280, 631648000),
                new YahooPrice(new DateTime(2020, 11, 01), 204.289993, 228.119995, 200.119995, 214.070007, 213.020401, 573443000),
                new YahooPrice(new DateTime(2020, 12, 01), 214.509995, 227.179993, 209.110001, 222.419998, 221.908905, 594806000),
                new YahooPrice(new DateTime(2021, 01, 01), 222.529999, 242.639999, 211.940002, 231.960007, 231.426987, 648098100),
                new YahooPrice(new DateTime(2021, 02, 01), 235.059998, 246.130005, 227.880005, 232.380005, 231.846024, 490925300),
                new YahooPrice(new DateTime(2021, 03, 01), 235.899994, 239.169998, 224.259995, 235.750000, 235.750000, 319043000),
                new YahooPrice(new DateTime(2021, 03, 12), 234.009995, 235.820007, 233.229996, 235.750000, 235.750000, 22653662),
            };

            return new YahooResult(prices, null);
        }
    }
}