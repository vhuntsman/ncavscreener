using System;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace NcavScreener
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            // Setup
            var parser = new Parser();
            var api = new Api();

            // Get the cookie and crumb
            var client = new RestClient("https://sg.finance.yahoo.com/screener/new");
            var crumb = parser.ExtractCrumb(api.GetAuthResponse(client));

            // Get the list of stocks
            var uri = "https://query1.finance.yahoo.com/v1/finance/screener?crumb=" + crumb + "&lang=en-SG&region=SG&formatted=true&corsDomain=sg.finance.yahoo.com";
            client = new RestClient(uri);
            JArray stockArray = api.GetStockList(client);

            // List stocks that are Price < NCAV
            foreach (var quote in stockArray)
            {
                // Get the symbol 
                string ticker = quote["symbol"].ToString();
                client = new RestClient("https://sg.finance.yahoo.com/quote/" + ticker + "/balance-sheet?p=" + ticker);
                var balanceSheet = api.GetStock(client);
                var ncav = parser.GetNcav(balanceSheet, ticker);
                var price = double.Parse(quote["regularMarketPrice"]["fmt"].ToString());
                string longName = string.Empty;
                try
                {
                    longName = quote["longName"].ToString();
                }
                catch
                {

                }
                var fullExchangeName = quote["fullExchangeName"];
                if (!double.IsNaN(ncav) && (ncav > 0.0d))
                {
                    if (price < ncav)
                    {
                        Console.WriteLine(String.Format("{0, 7} | {1, 8} | {2, 8} | {3, 60} | {4, 10}", ticker, price, ncav, longName, fullExchangeName));
                    }
                }
                // Rate limit the get requests
                Task.Delay(1000).GetAwaiter().GetResult();
            }
        }
    }
}
