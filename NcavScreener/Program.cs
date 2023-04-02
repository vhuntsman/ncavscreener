using System;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Diagnostics;

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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get the cookie and crumb
            var client = new RestClient("https://sg.finance.yahoo.com/screener/new");
            var crumb = parser.ExtractCrumb(api.GetAuthResponse(client));

            // Get the list of stocks
            var uri = "https://query1.finance.yahoo.com/v1/finance/screener?crumb=" + crumb + "&lang=en-SG&region=SG&formatted=true&corsDomain=sg.finance.yahoo.com";
            client = new RestClient(uri);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.67 Safari/537.36";

            JArray stockArray = api.GetStockList(client);

            using (StreamWriter writer = new StreamWriter($"output_{DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.txt"))
            {
                // List stocks that are Price < NCAV
                foreach (var quote in stockArray)
                {
                    // Get the symbol 
                    string ticker = quote["symbol"].ToString();
                    client = new RestClient("https://sg.finance.yahoo.com/quote/" + ticker + "/balance-sheet?p=" + ticker);
                    var balanceSheet = api.GetStock(client);
                    double sharesOutstanding = double.NaN;
                    try
                    {
                        sharesOutstanding = Convert.ToDouble(quote["sharesOutstanding"]["raw"]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }
                    var ncav = parser.GetNcav(balanceSheet, ticker, sharesOutstanding);
                    var price = double.Parse(quote["regularMarketPrice"]["fmt"].ToString());
                    string longName = string.Empty;
                    try
                    {
                        longName = quote["longName"].ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    var fullExchangeName = quote["fullExchangeName"];
                    if (!double.IsNaN(ncav) && (ncav > 0.0d))
                    {
                        if (price < ncav)
                        {
                            writer.WriteLine(String.Format("{0, 7} | {1, 8} | {2, 8} | {3, 60} | {4, 10}", ticker, price, ncav, longName, fullExchangeName));
                        }
                    }
                    // Rate limit the get requests
                    Task.Delay(2000).GetAwaiter().GetResult();
                }
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

        }
    }
}
