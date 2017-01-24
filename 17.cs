using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC2016_17
{
    internal static class Utils
    {
        public static string ByteToHexBitFiddle(byte[] bytes)
        {
            var c = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i] >> 4;
                c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            }
            return new string(c);
        }
    }
    internal class Vector : IEquatable<Vector>
    {
        public readonly int X;
        public readonly int Y;
        public char Direction { get; }
        public List<char> Path { get; }
        public string Password { get; set; }

        public Vector(int x, int y, string password)
        {
            X = x;
            Y = y;
            Path = new List<char>();
            Direction = '?';
            Password = password;
        }

        public Vector(int x, int y, List<char> path, char direction, string password)
        {
            X = x;
            Y = y;
            Path = new List<char>(path.ToArray()) {direction};
            Direction = direction;
            Password = password;
        }

        public bool Equals(Vector other)
        {
            return other != null && other.X == X && other.Y == Y;
        }

        public string Hash => Utils.ByteToHexBitFiddle(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(Password + new string(Path.ToArray()))));

        public override int GetHashCode()
        {
            return (23 * 37 + X) * 37 + Y;
        }
    }

    internal class Program
    {
        private readonly string _password;

        public Program(string password)
        {
            _password = password;
        }

        public string ShortestPath(Vector start, Vector end)
        {
            var queue = new Queue<Vector>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (current.Equals(end))
                {
                    return string.Join("", current.Path);
                }

                foreach (var move in ValidMoves(current.Hash, current))
                {
                    queue.Enqueue(move);
                }
            }

            return "";
        }

        public int LongestPath(Vector start, Vector end)
        {
            var queue = new Queue<Vector>();
            queue.Enqueue(start);

            var longestPath = 0;

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (current.Equals(end))
                {
                    if (current.Path.Count > longestPath)
                    {
                        longestPath = current.Path.Count;
                    }

                    continue;
                }

                foreach (var move in ValidMoves(current.Hash, current))
                {
                    queue.Enqueue(move);
                }
            }

            return longestPath;
        }

        private IEnumerable<Vector> ValidMoves(string hash, Vector current)
        {
            if (CanMove(hash[0]) && current.Y != 0)
                yield return new Vector(current.X, current.Y - 1, current.Path, 'U', _password);

            if (CanMove(hash[1]) && current.Y != 3)
                yield return new Vector(current.X, current.Y + 1, current.Path, 'D', _password);

            if (CanMove(hash[2]) && current.X != 0)
                yield return new Vector(current.X - 1, current.Y, current.Path, 'L', _password);

            if (CanMove(hash[3]) && current.X != 3)
                yield return new Vector(current.X + 1, current.Y, current.Path, 'R', _password);
        }

        private bool CanMove(char c)
        {
            return "bcdef".Contains(c);
        }

        public static void Dump(string path)
        {
            var grid = new[]
            {
                new[] {'.', '.', '.', '.'},
                new[] {'.', '.', '.', '.'},
                new[] {'.', '.', '.', '.'},
                new[] {'.', '.', '.', '.'},
            };

            {
                var x = 0;
                var y = 0;

                foreach (var p in path)
                {
                    grid[y][x] = 'O';
                    switch (p)
                    {
                        case 'U':
                            y--;
                            break;
                        case 'D':
                            y++;
                            break;
                        case 'L':
                            x--;
                            break;
                        case 'R':
                            x++;
                            break;
                    }
                }
            }

            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    Console.Write(grid[y][x]);
                }
                Console.WriteLine();
            }
        }

        private static void Main()
        {
            const string testPassword1 = "ihgpwlah";
            const string testPassword2 = "kglvqrro";
            const string testPassword3 = "ulqzkmiv";
            const string realPassword = "hhhxzeay";

            foreach (var testPassword in new[] { testPassword1, testPassword2, testPassword3, realPassword })
            {
                var start = new Vector(0, 0, testPassword);
                var end = new Vector(3, 3, testPassword);

                var testProgram = new Program(testPassword);

                Console.WriteLine("Shortest: " + testProgram.ShortestPath(start, end));
                Console.WriteLine("Longest: " + testProgram.LongestPath(start, end));
            }
        }
    }
}
