using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2016_22
{
    internal class Program
    {
        private static void Main()
        {
            var nodes = new AoCParser<Node>
                (
                    new Regex
                    (
                        @"
node
-x(?<X>[\d]+)
-y(?<Y>[\d]+)
\s+
(?<Size>[\d]+)T
\s+
(?<Used>[\d]+)T
\s+
(?<Avail>[\d]+)T
\s+
(?<Use>[\d]+)
%
",
                        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace
                    )
                )
                .Parse(Constants.Input)
                .ToArray();

            var count = nodes.Where(node => node.Used != 0)
                .SelectMany(node => nodes.Where(other => !other.Equals(node) && node.Used <= other.Avail))
                .Count();

            Console.WriteLine(count);
        }
    }
}
