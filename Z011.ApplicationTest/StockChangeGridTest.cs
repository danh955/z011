// <copyright file="StockChangeGridTest.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.ApplicationTest
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using Z011.Application;

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
            var query = new StockChangeGrid.Query();
            var handler = new StockChangeGrid.Handler();
            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
        }
    }
}