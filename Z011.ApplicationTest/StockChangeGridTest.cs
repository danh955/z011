// <copyright file="StockChangeGridTest.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ApplicationTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using Z011.Application;
    using Z011.Domain.Entities;
    using Z011.TestHelper;

    /// <summary>
    /// Stock change grid test class.
    /// </summary>
    public class StockChangeGridTest
    {
        /// <summary>
        /// Stock change grid handler test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task StockChangeGridHandler()
        {
            using var contextFactory = new InMemoryDbContextFactorySQLite();
            LoadDbWithTestData(contextFactory);

            var query = new StockChangeGrid.Query();
            var handler = new StockChangeGrid.Handler(contextFactory);
            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotNull(result.Headings);
            Assert.NotNull(result.Rows);
            Assert.Equal(5, result.Headings.Count());
            Assert.Equal(3, result.Rows.Count());
            Assert.Null(result.Rows.ElementAt(0).Percentages.First());
            Assert.NotNull(result.Rows.ElementAt(0).Percentages.ElementAt(1));
            Assert.NotNull(result.Rows.ElementAt(1).Percentages.First());
            Assert.NotNull(result.Rows.ElementAt(2).Percentages.First());
        }

        /// <summary>
        /// Load database with test data.
        /// </summary>
        /// <param name="contextFactory">InMemoryDbContextFactorySqlite.</param>
        private static void LoadDbWithTestData(InMemoryDbContextFactorySQLite contextFactory)
        {
            using var db = contextFactory.CreateDbContext();

            var stocks = new StockEntity[3]
            {
                new StockEntity() { Symbol = "ABC", Name = "ABC Stores" },
                new StockEntity() { Symbol = "DEF", Name = "DEF Industries" },
                new StockEntity() { Symbol = "GHI", Name = "GHI Stamps" },
            };

            db.Stocks.AddRange(stocks);
            db.SaveChanges();

            var rand = new Random();

            foreach (var stock in stocks)
            {
                List<StockPriceEntity> prices = new List<StockPriceEntity>();

                for (int i = 0; i < 5; i++)
                {
                    if (stock.Symbol == "ABC" && i == 0)
                    {
                        // skip ABC and 2020
                        continue;
                    }

                    int low = rand.Next(2000, 9999);
                    int high = rand.Next(low, low + 444);
                    int open = rand.Next(low, high);
                    int close = rand.Next(low, high);
                    int volume = rand.Next(1000000, 100000000);

                    prices.Add(new StockPriceEntity()
                    {
                        StockId = stock.Id,
                        Frequency = Frequency.Yearly,
                        Period = new DateTime(2020 + i, 1, 1),
                        Open = (double)open / 100,
                        Low = (double)low / 100,
                        High = (double)high / 100,
                        Close = (double)close / 100,
                        AdjClose = (double)close / 100,
                        Volume = volume,
                    });
                }

                db.StockPrices.AddRange(prices);
            }

            db.SaveChanges();
        }
    }
}