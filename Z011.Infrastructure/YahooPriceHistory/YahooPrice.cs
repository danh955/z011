// <copyright file="YahooPrice.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.YahooPriceHistory
{
    using System;

    /// <summary>
    /// Yahoo price data.
    /// </summary>
    public class YahooPrice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YahooPrice"/> class.
        /// </summary>
        /// <param name="date">Date of price.</param>
        /// <param name="open">Opening price.</param>
        /// <param name="high">High price.</param>
        /// <param name="low">Low price.</param>
        /// <param name="close">Closing price.</param>
        /// <param name="adjClose">Adjusted closing price.</param>
        /// <param name="volume">Volume.</param>
        public YahooPrice(DateTime date, double open, double high, double low, double close, double adjClose, int volume)
        {
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.AdjClose = adjClose;
            this.Volume = volume;
        }

        /// <summary>
        /// Gets date.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets open.
        /// </summary>
        public double Open { get; private set; }

        /// <summary>
        /// Gets high.
        /// </summary>
        public double High { get; private set; }

        /// <summary>
        /// Gets low.
        /// </summary>
        public double Low { get; private set; }

        /// <summary>
        /// Gets close.
        /// </summary>
        public double Close { get; private set; }

        /// <summary>
        /// Gets adjusted close.
        /// </summary>
        public double AdjClose { get; private set; }

        /// <summary>
        /// Gets volume.
        /// </summary>
        public int Volume { get; private set; }
    }
}