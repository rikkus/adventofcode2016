using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc2016_15
{
    struct Disc
    {
        public int Count { get; set; }
        public int Value { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var testInput = @"Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1.".Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var realInput = @"Disc #1 has 5 positions; at time=0, it is at position 2.
Disc #2 has 13 positions; at time=0, it is at position 7.
Disc #3 has 17 positions; at time=0, it is at position 10.
Disc #4 has 3 positions; at time=0, it is at position 2.
Disc #5 has 19 positions; at time=0, it is at position 9.
Disc #6 has 7 positions; at time=0, it is at position 0.".Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var input = realInput;

            var expr = new Regex(@"^Disc #(?<n>\d+) has (?<count>\d+) positions; at time=0, it is at position (?<initial>\d+).$");
            var discs = input.Select(line =>
                {
                    var match = expr.Match(line);

                    var n = int.Parse(match.Groups["n"].Value);
                    var count = int.Parse(match.Groups["count"].Value);
                    var initial = int.Parse(match.Groups["initial"].Value);

                    return new Disc {Count = count, Value = initial};
                })
                .ToList();

            var second = 0;

            while (true)
            {
                var wouldBeZeroCount = 0;

                for (var discIndex = 0; discIndex < discs.Count; discIndex++)
                {
                    var projectedSecond = discIndex + second + 1;
                    var disc = discs[discIndex];

                    var wouldBeZero =
                        (disc.Value + projectedSecond)
                        %
                        disc.Count
                        == 0;
                    
                    if (wouldBeZero)
                        wouldBeZeroCount++;

                    disc.Value++;
                    if (disc.Value == disc.Count)
                        disc.Value = 0;
                }

                if (wouldBeZeroCount == discs.Count)
                {
                    Console.WriteLine(second);
                    Console.ReadKey();
                    return;
                }
                second++;
            }

            // 148737?
        }

        private void DumpDiscs(IEnumerable<Disc> discs)
        {
            foreach (var disc in discs)
            {
                Console.WriteLine($"{disc.Value}\t{disc.Count}");
            }

            Console.WriteLine("----");
        }
    }
}
