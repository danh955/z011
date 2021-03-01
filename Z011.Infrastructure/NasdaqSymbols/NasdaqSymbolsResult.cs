// <copyright file="NasdaqSymbolsResult.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.NasdaqSymbols
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result from the NASDAQ symbols query.
    /// </summary>
    public class NasdaqSymbolsResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NasdaqSymbolsResult"/> class.
        /// </summary>
        /// <param name="nasdaqSymbols">List of NASDAQ symbols.</param>
        /// <param name="nasdaqSymbolsFileCreationTime">NASDAQ symbols file creation time.</param>
        /// <param name="otherSymbols">List of other symbols.</param>
        /// <param name="otherSymbolsFileCreationTime">Other symbols file creation time.</param>
        public NasdaqSymbolsResult(IEnumerable<NasdaqSymbolDto> nasdaqSymbols, DateTime nasdaqSymbolsFileCreationTime, IEnumerable<OtherSymbolDto> otherSymbols, DateTime otherSymbolsFileCreationTime)
        {
            this.NasdaqSymbols = nasdaqSymbols;
            this.NasdaqSymbolsFileCreationTime = nasdaqSymbolsFileCreationTime;
            this.OtherSymbols = otherSymbols;
            this.OtherSymbolsFileCreationTime = otherSymbolsFileCreationTime;
        }

        /// <summary>
        /// Gets list of items.
        /// </summary>
        public IEnumerable<NasdaqSymbolDto> NasdaqSymbols { get; private set; }

        /// <summary>
        /// Gets NASDAQ symbols file creation time.
        /// </summary>
        public DateTime NasdaqSymbolsFileCreationTime { get; private set; }

        /// <summary>
        /// Gets list of items.
        /// </summary>
        public IEnumerable<OtherSymbolDto> OtherSymbols { get; private set; }

        /// <summary>
        /// Gets NASDAQ symbols file creation time.
        /// </summary>
        public DateTime OtherSymbolsFileCreationTime { get; private set; }
    }
}