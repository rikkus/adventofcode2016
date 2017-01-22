using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2016_13
{
    internal class Vector : IEquatable<Vector>
    {
        public readonly int X;
        public readonly int Y;
        public Vector Previous { get; set; }
        public int Distance { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector other)
        {
            return other != null && other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return (23*37 + X)*37 + Y; 
        }
    }

    class Program
    {
        private readonly int _input;
        private readonly Dictionary<Vector, bool> _walls = new Dictionary<Vector, bool>();

        public Program(int i)
        {
            _input = i;
        }

        public IEnumerable<Vector> ShortestPath(Vector target)
        {
            var start = new Vector(1, 1);

            var seen = new HashSet<Vector>();

            var queue = new Queue<Vector>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (current.Equals(target))
                {
                    var c = current;

                    while (c != start)
                    {
                        yield return c;
                        c = c.Previous;
                    }

                    yield break;
                }

                foreach (var possibleMove in PossibleMoves(current, seen))
                {
                    possibleMove.Previous = current;
                    queue.Enqueue(possibleMove);
                }

                seen.Add(current);
            }
        }

        public IEnumerable<Vector> ReachableCells(int steps)
        {
            var start = new Vector(1, 1);

            var seen = new HashSet<Vector>();

            var queue = new Queue<Vector>();
            queue.Enqueue(start);

            var x = new List<Vector>();

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (current.Distance <= steps)
                {
                    x.Add(current);
                }

                foreach (var possibleMove in PossibleMoves(current, seen))
                {
                    queue.Enqueue(new Vector(possibleMove.X, possibleMove.Y) { Distance = current.Distance + 1});
                }

                seen.Add(current);
            }

            return x;
        }

        private IEnumerable<Vector> PossibleMoves(Vector current, ICollection<Vector> seen)
        {
            return new[]
                {
                    new Vector(current.X - 1, current.Y),
                    new Vector(current.X + 1, current.Y),
                    new Vector(current.X, current.Y - 1),
                    new Vector(current.X, current.Y + 1)
                }
                .Where(v => !seen.Contains(v) && IsValid(v));
        }

        private static TTestResult Cache<T, TTestResult>(T item, IDictionary<T, TTestResult> cache, Func<T, TTestResult> test)
        {
            TTestResult cached;
            if (cache.TryGetValue(item, out cached))
                return cached;
            cached = test(item);
            cache[item] = cached;
            return cached;
        }

        private static bool IsOutOfBounds(Vector v)
        {
            return v.X == -1 || v.Y == -1;
        }

        private bool IsWall(Vector v)
        {
            return (CountBits(v.X*v.X + 3*v.X + 2*v.X*v.Y + v.Y + v.Y*v.Y + _input) & 1) == 1;
        }

        private bool IsValid(Vector cell)
        {
            return !IsOutOfBounds(cell) && !Cache(cell, _walls, IsWall);
        }

        private static int CountBits(int v)
        {
            // Specialised minimum-instruction-count bit-twiddling method for 32 bit representations.
            v = v - ((v >> 1) & 0x55555555);
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
            return ((v + (v >> 4) & 0xF0F0F0F)*0x1010101) >> 24;
        }

        public void Dump(Vector size, ICollection<Vector> path)
        {
            for (var y = 0; y < size.Y; y++)
            {
                for (var x = 0; x < size.X; x++)
                {
                    var v = new Vector(x, y);
                    var pathV = path.FirstOrDefault(pv => pv.Equals(v));
                    Console.Write(IsWall(v) ? "##" : (pathV?.Distance.ToString().PadLeft(2, '0') ?? "  "));
                }
                Console.WriteLine();
            } 
        } 

        private static void Main()
        {
            {
                var testProgram = new Program(10);
                var shortestPathTest = testProgram.ShortestPath(new Vector(7, 4)).ToArray();
                testProgram.Dump(new Vector(12, 12), new HashSet<Vector>(shortestPathTest));
                Console.WriteLine(shortestPathTest.Length);
            }

            Console.WriteLine("----------------------------------------------------------------");

            {
                var realProgram = new Program(1364);
                var shortestPathReal = realProgram.ShortestPath(new Vector(31, 39)).ToArray();
                realProgram.Dump(new Vector(40, 45), shortestPathReal);
                Console.WriteLine(shortestPathReal.Length);
                Console.WriteLine("----------------------------------------------------------------");
                var reachableCellsReal = realProgram.ReachableCells(50).ToArray();
                realProgram.Dump(new Vector(40, 45), reachableCellsReal);
                Console.WriteLine(reachableCellsReal.Distinct().Count());
            }
        }
    }
}
