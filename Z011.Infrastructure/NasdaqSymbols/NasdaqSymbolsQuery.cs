// <copyright file="NasdaqSymbolsQuery.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.NasdaqSymbols
{
    using MediatR;

    /// <summary>
    /// Get all the NASDAQ symbols and others.
    /// </summary>
    public class NasdaqSymbolsQuery : IRequest<NasdaqSymbolsResult>
    {
    }
}