using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NcavScreener
{
    public class Parser
    {
        private readonly string context;

        public Parser(string context)
        {
            this.context = context;
        }

        public List<string> ExtractPageLinks()
        { 
            var results = new List<string>();
            // Define a regular expression for repeated words.
            Regex td = new Regex(@"<td .* class=""body-table screener_pagination"".*?>(.*?)<\/td>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var tdContext = td.Matches(context)?[0].Groups[1].Value;
            Regex links = new Regex(@"href=""(.*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = links.Matches(tdContext);
            foreach (Match match in matches)
            {
                results.Add(match.Groups[1].Value);
            }
            // results.RemoveAt(0);
            results.RemoveAt(results.Count - 1);
            return results;
        }
    }
}
