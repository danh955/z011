// <copyright file="YahooResult.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.YahooPriceHistory
{
    using System.Collections.Generic;

    /// <summary>
    /// Yahoo result class.
    /// </summary>
    public class YahooResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooResult"/> class.
        /// </summary>
        /// <param name="prices">List of Yahoo prices.</param>
        /// <param name="errorMessage">Error message.  Null if successful.</param>
        public YahooResult(IEnumerable<YahooPrice> prices, string errorMessage = null)
        {
            this.Prices = prices;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets list of Yahoo prices.
        /// </summary>
        public IEnumerable<YahooPrice> Prices { get; private set; }

        /// <summary>
        /// Gets the error message.  Null if successful.
        /// </summary>
        public string ErrorMessage { get; private set; }
    }
}