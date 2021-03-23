// <copyright file="NasdaqSymbolsTest.cs" company="None">
// Free and open source code.
// </copyright>

namespace Z011.InfrastructureTest
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using Z011.Infrastructure.NasdaqSymbols;
    using Z011.InfrastructureTest.Helper;

    /// <summary>
    /// Test NASDAQ and other symbols class.
    /// </summary>
    public class NasdaqSymbolsTest
    {
        /// <summary>
        /// Handler test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task NasdaqSymbolsHandlerTest()
        {
            var handler = new NasdaqSymbolsHandler(MockData.MockIHttpClientFactoryForNasdaqSymbols());
            var result = await handler.Handle(new NasdaqSymbolsQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotNull(result.NasdaqSymbols);
            Assert.NotNull(result.OtherSymbols);
            Assert.True(result.NasdaqSymbols.Any());
            Assert.True(result.OtherSymbols.Any());
        }
    }
}