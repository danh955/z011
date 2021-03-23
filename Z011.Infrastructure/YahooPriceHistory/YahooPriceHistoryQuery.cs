// <copyright file="YahooPriceHistoryQuery.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.YahooPriceHistory
{
    using System;
    using MediatR;

    /// <summary>
    /// Yahoo price history query class.
    /// </summary>
    public class YahooPriceHistoryQuery : IRequest<YahooResult>
    {
        /// <summary>
        /// Gets or sets symbol of prices to get.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the first date.
        /// </summary>
        public DateTime FirstDate { get; set; }

        /// <summary>
        /// Gets or sets the last date.
        /// </summary>
        public DateTime LastDate { get; set; }

        /// <summary>
        /// Gets or sets the yahoo stock price interval.
        /// </summary>
        public YahooInterval Interval { get; set; }
    }
}