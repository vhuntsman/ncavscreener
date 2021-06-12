using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NcavScreener
{
    public class Api
    {
        public string SessionCookie { get; private set; }
        private string _sessionCookie;
        private RestRequest _request;

        public string GetAuthResponse(IRestClient client)
        {
            client.Timeout = -1;
            _request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(_request);
            SessionCookie = response?.Cookies[0]?.Name + "=" + response?.Cookies[0]?.Value;
            _sessionCookie = response?.Cookies[0]?.Name + "=" + response?.Cookies[0]?.Value;
            return response?.Content;
        }

        public JArray GetStockList(IRestClient client)
        {
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Cookie", _sessionCookie);
            var queryOffset = 0;
            var body = @"{""size"":250,""offset"":" + queryOffset + @",""sortField"":""intradayprice"",""sortType"":""asc"",""quoteType"":""EQUITY"",""topOperator"":""AND"",""query"":{""operator"":""AND"",""operands"":[{""operator"":""or"",""operands"":[{""operator"":""EQ"",""operands"":[""region"",""us""]},{""operator"":""EQ"",""operands"":[""region"",""hk""]},{""operator"":""EQ"",""operands"":[""region"",""sg""]},{""operator"":""EQ"",""operands"":[""region"",""ca""]},{""operator"":""EQ"",""operands"":[""region"",""au""]},{""operator"":""EQ"",""operands"":[""region"",""jp""]},{""operator"":""EQ"",""operands"":[""region"",""gb""]},{""operator"":""EQ"",""operands"":[""region"",""fr""]},{""operator"":""EQ"",""operands"":[""region"",""fr""]},{""operator"":""EQ"",""operands"":[""region"",""de""]},{""operator"":""EQ"",""operands"":[""region"",""ch""]},{""operator"":""EQ"",""operands"":[""region"",""nl""]},{""operator"":""EQ"",""operands"":[""region"",""be""]},{""operator"":""EQ"",""operands"":[""region"",""lu""]},{""operator"":""EQ"",""operands"":[""region"",""at""]},{""operator"":""EQ"",""operands"":[""region"",""cz""]},{""operator"":""EQ"",""operands"":[""region"",""fi""]},{""operator"":""EQ"",""operands"":[""region"",""hu""]},{""operator"":""EQ"",""operands"":[""region"",""pl""]},{""operator"":""EQ"",""operands"":[""region"",""ru""]},{""operator"":""EQ"",""operands"":[""region"",""gr""]},{""operator"":""EQ"",""operands"":[""region"",""it""]},{""operator"":""EQ"",""operands"":[""region"",""pt""]},{""operator"":""EQ"",""operands"":[""region"",""es""]}]},{""operator"":""lt"",""operands"":[""totaldebtequity.lasttwelvemonths"",25]},{""operator"":""or"",""operands"":[{""operator"":""EQ"",""operands"":[""sector"",""Consumer Defensive""]},{""operator"":""EQ"",""operands"":[""sector"",""Utilities""]},{""operator"":""EQ"",""operands"":[""sector"",""Technology""]},{""operator"":""EQ"",""operands"":[""sector"",""Consumer Cyclical""]},{""operator"":""EQ"",""operands"":[""sector"",""Communication Services""]},{""operator"":""EQ"",""operands"":[""sector"",""Healthcare""]},{""operator"":""EQ"",""operands"":[""sector"",""Industrials""]},{""operator"":""EQ"",""operands"":[""sector"",""Energy""]}]},{""operator"":""lt"",""operands"":[""lastclosepricebookvalue.lasttwelvemonths"",1]}]},""userId"":"""",""userIdType"":""guid""}";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic stocks = JsonConvert.DeserializeObject(response.Content);
            // Get the count of stocks
            var stockCount = stocks["finance"]["result"][0]["total"].Value;

            // Master Jarray
            var stockArray = stocks["finance"]["result"][0]["quotes"];

            queryOffset += 250;
            while (queryOffset < stockCount)
            {
                request.Parameters.Clear();
                request.AddHeader("Content-Type", "text/plain");
                request.AddHeader("Cookie", _sessionCookie);
                body = @"{""size"":250,""offset"":" + queryOffset + @",""sortField"":""intradayprice"",""sortType"":""asc"",""quoteType"":""EQUITY"",""topOperator"":""AND"",""query"":{""operator"":""AND"",""operands"":[{""operator"":""or"",""operands"":[{""operator"":""EQ"",""operands"":[""region"",""us""]},{""operator"":""EQ"",""operands"":[""region"",""hk""]},{""operator"":""EQ"",""operands"":[""region"",""sg""]},{""operator"":""EQ"",""operands"":[""region"",""ca""]},{""operator"":""EQ"",""operands"":[""region"",""au""]},{""operator"":""EQ"",""operands"":[""region"",""jp""]},{""operator"":""EQ"",""operands"":[""region"",""gb""]},{""operator"":""EQ"",""operands"":[""region"",""fr""]},{""operator"":""EQ"",""operands"":[""region"",""fr""]},{""operator"":""EQ"",""operands"":[""region"",""de""]},{""operator"":""EQ"",""operands"":[""region"",""ch""]},{""operator"":""EQ"",""operands"":[""region"",""nl""]},{""operator"":""EQ"",""operands"":[""region"",""be""]},{""operator"":""EQ"",""operands"":[""region"",""lu""]},{""operator"":""EQ"",""operands"":[""region"",""at""]},{""operator"":""EQ"",""operands"":[""region"",""cz""]},{""operator"":""EQ"",""operands"":[""region"",""fi""]},{""operator"":""EQ"",""operands"":[""region"",""hu""]},{""operator"":""EQ"",""operands"":[""region"",""pl""]},{""operator"":""EQ"",""operands"":[""region"",""ru""]},{""operator"":""EQ"",""operands"":[""region"",""gr""]},{""operator"":""EQ"",""operands"":[""region"",""it""]},{""operator"":""EQ"",""operands"":[""region"",""pt""]},{""operator"":""EQ"",""operands"":[""region"",""es""]}]},{""operator"":""lt"",""operands"":[""totaldebtequity.lasttwelvemonths"",25]},{""operator"":""or"",""operands"":[{""operator"":""EQ"",""operands"":[""sector"",""Consumer Defensive""]},{""operator"":""EQ"",""operands"":[""sector"",""Utilities""]},{""operator"":""EQ"",""operands"":[""sector"",""Technology""]},{""operator"":""EQ"",""operands"":[""sector"",""Consumer Cyclical""]},{""operator"":""EQ"",""operands"":[""sector"",""Communication Services""]},{""operator"":""EQ"",""operands"":[""sector"",""Healthcare""]},{""operator"":""EQ"",""operands"":[""sector"",""Industrials""]},{""operator"":""EQ"",""operands"":[""sector"",""Energy""]}]},{""operator"":""lt"",""operands"":[""lastclosepricebookvalue.lasttwelvemonths"",1]}]},""userId"":"""",""userIdType"":""guid""}";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse loopResponse = client.Execute(request);
                dynamic loopStocks = JsonConvert.DeserializeObject(loopResponse.Content);
                foreach (var quote in loopStocks["finance"]["result"][0]["quotes"])
                {
                    stockArray.Add(quote);
                }
                queryOffset += 250;
            }
            return stockArray;
        }

        public string GetStock(IRestClient client)
        {
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", _sessionCookie);
            IRestResponse response = client.Execute(request);
            return response?.Content;
        }
    }
}
