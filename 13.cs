using System;
using System.Linq;

namespace CrazyMaze
{
    internal class Program
    {
        private const int Input = 1364;

        private static double Heuristic(INode a, INode b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);            
        }

        public static void Main()
        {
            var maze = new Maze(Input);
            
            var startingNode = new Node(maze, 1, 1);
            var finishingNode = new Node(maze, 31, 39);


            // Standard A* algorithm. Nicked from Eric Lippert. Unfortunately it doesn't give the correct answer.
            
            var path = PathFinder.FindPath
            (
                startingNode,
                finishingNode,
                (_1, _2) => 1,
                Heuristic
            );

            DumpMaze(maze, path, 48, 48);

            // 88 is too high :(

            Console.WriteLine(path.Count() + 1);
            Console.ReadKey();
        }

        private static void DumpMaze(Maze maze, Path path, int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (maze.Cell(x, y) == null)
                    {
                        Console.Write('#');
                    }
                    else if (path != null && path.Any(node => node.X == x && node.Y == y))
                    {
                        Console.Write('O');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
