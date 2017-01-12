using System;
using System.Diagnostics;

namespace Aoc2016_11
{
    internal class Program
    {
        private static void DumpSolution(Tracker tracker)
        {
            var current = tracker;

            while (current != null)
            {
                Console.WriteLine(current.State.ToString());
                Console.WriteLine("----");
                current = current.Previous;
            }
        }
        private static void Main()
        {
            var s = Stopwatch.StartNew();
            var solution = new Solver().Solve(State.Parse(Constants.RealInputTwo));

            s.Stop();
            Console.WriteLine(solution.Depth);
            DumpSolution(solution);
            Console.WriteLine($"Solved in {solution.Depth} moves, {s.Elapsed}");
            Console.ReadKey();
        }
    }
}
