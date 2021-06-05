using System;
using RestSharp;

namespace NcavScreener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var netCoreVer = System.Environment.Version; // 3.0.0
            var runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription; // .NET Core 3.0.0-preview4.19113.15
            Console.WriteLine(runtimeVer);
            var client = new RestClient("https://finviz.com/screener.ashx?v=111&f=fa_curratio_o1.5,fa_debteq_u0.5,fa_pb_low&ft=2&o=price"); // TODO: make criteria configurable in console or use default values
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "screenerUrl=screener.ashx%3Fv%3D111%26f%3Dfa_curratio_o1.5%2Cfa_debteq_u0.5%2Cfa_pb_low%26ft%3D2%26o%3Dprice");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }
    }
}
