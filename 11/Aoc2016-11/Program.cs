using System;

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
            var solution = new Solver().Solve(State.Parse(Constants.RealInputTwo));

            Console.WriteLine(solution.Depth);
            DumpSolution(solution);
            Console.ReadKey();
        }
    }
}
