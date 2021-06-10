using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NcavScreener
{
    public class Parser
    {
        public string ExtractCrumb(string context)
        {
            // Pattern for crumb
            Regex re = new Regex(@"""CrumbStore"":{""crumb"":""(.*?)""}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var matches = re.Matches(context);
            return Regex.Unescape(matches[0].Groups[1].Value);
        }

        public void FindStocks(string context)
        {

        }

        public double GetNcav(string context)
        {
            double result = double.NaN;
            Regex re = new Regex(@"title=""Total current assets"".*?<\/div><div class.*?<\/div><\/div><div class.*?><span.*?>(.*?)<\/span>.*?title=""Total liabilities"".*?<\/div><div class.*?<\/div><\/div><div class.*?><span.*?>(.*?)<\/span>.*?title=""Common stock"".*?<\/div><div class.*?<\/div><\/div><div class.*?><span.*?>(.*?)<\/span>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var matches = re.Matches(context);
            try
            {
                var currentAssets = double.Parse(matches[0].Groups[1].Value);
                var totalLiabilities = double.Parse(matches[0].Groups[2].Value);
                var numCommonShares = double.Parse(matches[0].Groups[3].Value);
                result = (currentAssets - totalLiabilities) / numCommonShares;
                result = Math.Round(result, 2);
            }
            catch
            {

            }
            return result;
        }
    }
}
