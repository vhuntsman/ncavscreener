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
            request2.AddParameter("text/plain", "{\"size\":250,\"offset\":" + queryOffset +",\"sortField\":\"intradayprice\",\"sortType\":\"asc\",\"quoteType\":\"EQUITY\",\"topOperator\":\"AND\",\"query\":{\"operator\":\"AND\",\"operands\":[{\"operator\":\"or\",\"operands\":[{\"operator\":\"EQ\",\"operands\":[\"region\",\"sg\"]}]},{\"operator\":\"lt\",\"operands\":[\"lastclosepricebookvalue.lasttwelvemonths\",1]},{\"operator\":\"gt\",\"operands\":[\"currentratio.lasttwelvemonths\",1.5]},{\"operator\":\"lt\",\"operands\":[\"totaldebtequity.lasttwelvemonths\",50]}]},\"userId\":\"\",\"userIdType\":\"guid\"}", ParameterType.RequestBody);
            IRestResponse response2 = client2.Execute(request2);
            dynamic stocks = JsonConvert.DeserializeObject(response2.Content);

            // Get the count of stocks
            var stockCount = stocks["finance"]["result"][0]["count"];

            // list all stocks and price
            foreach(var item in stocks["finance"]["result"][0]["quotes"])
            {
                Console.WriteLine(item["symbol"] + '\t' + item["regularMarketPrice"]["fmt"]);
            }
        }
    }
}
