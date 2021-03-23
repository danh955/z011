// <copyright file="YahooPriceHistoryTest.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.InfrastructureTest
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using Z011.Infrastructure.YahooPriceHistory;
    using Z011.InfrastructureTest.Helper;

    /// <summary>
    /// Test Yahoo price history.
    /// </summary>
    public class YahooPriceHistoryTest
    {
        /// <summary>
        /// Handler test.
        /// </summary>
        /// <param name="firstDate">The first date.</param>
        /// <param name="lastDate">The last date.</param>
        /// <param name="interval">Interval of the data.</param>
        /// <param name="count">Expected number of rows returned.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("3/9/2021", "3/14/2021", YahooInterval.Daily, 4)]
        [InlineData("12/14/2020", "3/14/2021", YahooInterval.Weekly, 14)]
        [InlineData("10/1/2021", "3/14/2021", YahooInterval.Monthly, 7)]
        public async Task YahooPriceHistoryHandlerTest(string firstDate, string lastDate, YahooInterval interval, int count)
        {
            var query = new YahooPriceHistoryQuery()
            {
                Symbol = "MSFT",
                FirstDate = DateTime.Parse(firstDate),
                LastDate = DateTime.Parse(lastDate),
                Interval = interval,
            };

            var handler = new YahooPriceHistoryHandler(YahooPriceHistoryHelper.MockIHttpClientFactory());
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.Prices);
            Assert.True(result.Prices.Any());
            Assert.Equal(count, result.Prices.Count());
        }
    }
}