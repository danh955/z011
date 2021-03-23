// <copyright file="YahooPriceHistoryHandler.cs" company="None">
// Free and open source code.
// </copyright>
namespace Z011.Infrastructure.YahooPriceHistory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using CsvHelper;
    using CsvHelper.Configuration;
    using MediatR;

    /// <summary>
    /// Yahoo price history handler class.
    /// </summary>
    public class YahooPriceHistoryHandler : IRequestHandler<YahooPriceHistoryQuery, YahooResult>
    {
        private readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0);

        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooPriceHistoryHandler"/> class.
        /// </summary>
        /// <param name="httpClientFactory">IHttpClientFactory.</param>
        public YahooPriceHistoryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get stock history data from Yahoo.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>YahooResult.</returns>
        public async Task<YahooResult> Handle(YahooPriceHistoryQuery request, CancellationToken cancellationToken)
        {
            string symbol = request.Symbol;
            long epochFirstDate = (long)(request.FirstDate.Date - this.epoch).TotalSeconds;
            long epochLastDate = (long)(request.LastDate.Date - this.epoch).TotalSeconds;
            string interval = request.Interval switch
            {
                YahooInterval.Daily => "1d",
                YahooInterval.Weekly => "1wk",
                YahooInterval.Monthly => "1mo",
                YahooInterval.Quorterly => "3mo",
                _ => throw new NotImplementedException(request.Interval.ToString()),
            };

            string uri = $"https://query1.finance.yahoo.com/v7/finance/download/{symbol}?period1={epochFirstDate}&period2={epochLastDate}&interval={interval}&events=history&includeAdjustedClose=true";

            List<YahooPrice> items = new();

            try
            {
                HttpClient httpClient = this.httpClientFactory.CreateClient();
                using var responseStream = await httpClient.GetStreamAsync(uri, cancellationToken);
                using var streamReader = new StreamReader(responseStream);
                using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));

                if (!cancellationToken.IsCancellationRequested && await csv.ReadAsync())
                {
                    csv.ReadHeader();

                    while (!cancellationToken.IsCancellationRequested && await csv.ReadAsync())
                    {
                        items.Add(new YahooPrice(
                            date: csv.GetField<DateTime>(0),
                            open: csv.GetField<double>(1),
                            high: csv.GetField<double>(2),
                            low: csv.GetField<double>(3),
                            close: csv.GetField<double>(4),
                            adjClose: csv.GetField<double>(5),
                            volume: csv.GetField<int>(6)));
                    }
                }
            }
            catch (HttpRequestException e)
            {
                return new YahooResult(null, e.Message);
            }

            return new YahooResult(items);
        }
    }
}

//// https://help.yahoo.com/kb/download-historical-data-yahoo-finance-sln2311.html.
//// https://query1.finance.yahoo.com/v7/finance/download/MSFT?period1=1615248000&period2=1615680000&interval=1d&events=history&includeAdjustedClose=true.
//// https://query1.finance.yahoo.com/v7/finance/download/{ticker}?period1={start_time}&period2={end_time}&interval={interval}&events=history
//// https://query1.finance.yahoo.com/v7/finance/download/msft?period1=1609459200&period2=1615429161&interval=1d&events=history
//// https://stackoverflow.com/questions/2883576/how-do-you-convert-epoch-time-in-c