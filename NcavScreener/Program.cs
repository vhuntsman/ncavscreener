using System;
using RestSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NcavScreener
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Mock the rest clients
            // Get the cookie and crumb
            var client = new RestClient("https://sg.finance.yahoo.com/screener/new");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            
            // Get the session cookie
            var sessionCookie = response.Cookies[0].Name + "=" + response.Cookies[0].Value;

            // Get the crumb
            var parser = new Parser();
            var crumb = parser.ExtractCrumb(response.Content);


            // Get the list of stocks
            var uri = "https://query1.finance.yahoo.com/v1/finance/screener?crumb=" + crumb + "&lang=en-SG&region=SG&formatted=true&corsDomain=sg.finance.yahoo.com";
            var client2 = new RestClient(uri);
            client2.Timeout = -1;
            var request2 = new RestRequest(Method.POST);
            request2.AddHeader("Content-Type", "text/plain");
            request2.AddHeader("Cookie", sessionCookie);
            var queryOffset = 0;
            request2.AddParameter("text/plain", "{\"size\":250,\"offset\":" + queryOffset +",\"sortField\":\"intradayprice\",\"sortType\":\"asc\",\"quoteType\":\"EQUITY\",\"topOperator\":\"AND\",\"query\":{\"operator\":\"AND\",\"operands\":[{\"operator\":\"or\",\"operands\":[{\"operator\":\"EQ\",\"operands\":[\"region\",\"us\"]}]},{\"operator\":\"lt\",\"operands\":[\"lastclosepricebookvalue.lasttwelvemonths\",1]},{\"operator\":\"gt\",\"operands\":[\"currentratio.lasttwelvemonths\",1.5]},{\"operator\":\"lt\",\"operands\":[\"totaldebtequity.lasttwelvemonths\",50]}]},\"userId\":\"\",\"userIdType\":\"guid\"}", ParameterType.RequestBody);
            IRestResponse response2 = client2.Execute(request2);
            dynamic stocks = JsonConvert.DeserializeObject(response2.Content);

            // Get the count of stocks
            var stockCount = stocks["finance"]["result"][0]["total"];

            // Master Jarray
            var stockArray = stocks["finance"]["result"][0]["quotes"];

            queryOffset += 250;
            while (queryOffset < stockCount.Value)
            {
                request2.Parameters.Clear();
                request2.AddHeader("Content-Type", "text/plain");
                request2.AddHeader("Cookie", sessionCookie);
                request2.AddParameter("text/plain", "{\"size\":250,\"offset\":" + queryOffset + ",\"sortField\":\"intradayprice\",\"sortType\":\"asc\",\"quoteType\":\"EQUITY\",\"topOperator\":\"AND\",\"query\":{\"operator\":\"AND\",\"operands\":[{\"operator\":\"or\",\"operands\":[{\"operator\":\"EQ\",\"operands\":[\"region\",\"us\"]}]},{\"operator\":\"lt\",\"operands\":[\"lastclosepricebookvalue.lasttwelvemonths\",1]},{\"operator\":\"gt\",\"operands\":[\"currentratio.lasttwelvemonths\",1.5]},{\"operator\":\"lt\",\"operands\":[\"totaldebtequity.lasttwelvemonths\",50]}]},\"userId\":\"\",\"userIdType\":\"guid\"}", ParameterType.RequestBody);
                IRestResponse loopResponse = client2.Execute(request2);
                dynamic loopStocks = JsonConvert.DeserializeObject(loopResponse.Content);
                foreach(var quote in loopStocks["finance"]["result"][0]["quotes"])
                {
                    stockArray.Add(quote);
                }
                queryOffset += 250;
            }

            // List stocks that are Price < NCAV
            foreach (var quote in stockArray)
            {
                // Get the symbol 
                string ticker = quote["symbol"].Value;
                var balanceSheetClient = new RestClient("https://sg.finance.yahoo.com/quote/" + ticker + "/balance-sheet?p=" + ticker);
                balanceSheetClient.Timeout = -1;
                var balanceSheetRequest = new RestRequest(Method.GET);
                balanceSheetRequest.AddHeader("Cookie", sessionCookie);
                IRestResponse balanceSheetResponse = balanceSheetClient.Execute(balanceSheetRequest);
                var ncav = parser.GetNcav(balanceSheetResponse.Content);
                var price = double.Parse(quote["regularMarketPrice"]["fmt"].Value);
                var longName = quote["longName"].Value;
                var fullExchangeName = quote["fullExchangeName"];
                if (!double.IsNaN(ncav) && (ncav > 0.0d))
                {
                    if (ncav < price)
                    {
                        Console.WriteLine(String.Format("{0,5} | {1, 8} | {2, 8} | {3, 30} | {4, 20}", ticker, ncav, price, longName, fullExchangeName));
                        // Console.WriteLine(ticker + "\t" + ncav + "\t" + string.Format("{0, 7}", price) + "\t\t" + longName + "\t" + fullExchangeName);
                    }
                }
            }
        }
    }
}
