// <copyright file="StockChangeGrid.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Application
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    /// <summary>
    /// Stock change grid class.
    /// This will get a list of stock and each column will be each years percentage change.
    /// </summary>
    public static class StockChangeGrid
    {
        /// <summary>
        /// Get nothing query.
        /// </summary>
        public class Query : IRequest<Result>
        {
        }

        /// <summary>
        /// Stock change grid result.
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Gets headings for the percentages.
            /// </summary>
            public IEnumerable<string> Headings { get; internal set; }

            /// <summary>
            /// Gets list of rows.
            /// </summary>
            public IEnumerable<Row> Rows { get; internal set; }
        }

        /// <summary>
        /// Each row of symbol and percentages class.
        /// </summary>
        public class Row
        {
            /// <summary>
            /// Gets stock symbol for row.
            /// </summary>
            public string Symbol { get; internal set; }

            /// <summary>
            /// Gets a list of percentages for the row.
            /// </summary>
            public IEnumerable<double> Percentages { get; internal set; }
        }

        /// <summary>
        /// Handler class.
        /// </summary>
        public class Handler : IRequestHandler<Query, Result>
        {
            /// <inheritdoc/>
            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                //// TODO: do something.
                return await Task.FromResult<Result>(new Result());
            }
        }
    }
}