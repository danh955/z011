// <copyright file="StockPriceUpdaterCommand.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Application.StockPriceUpdater
{
    using MediatR;
    using Z011.Domain.Entities;

    /// <summary>
    /// Update the database stock price table from the Yahoo finance.
    /// </summary>
    public class StockPriceUpdaterCommand : IRequest
    {
        /// <summary>
        /// Gets or sets the frequency of the prices.
        /// </summary>
        public Frequency Frequency { get; set; }
    }
}