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
    }
}
