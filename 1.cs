
using System;
using System.Collections.Generic;
using System.Linq;

internal class Program
{
    private struct Vector
    {
        public bool IsValid;
        public int X { get; }
        public int Y { get; }

        public Vector(int x, int y)
        {
            IsValid = true;
            X = x;
            Y = y;
        }

        public int DistanceTo(Vector other)
        {
            return Math.Abs(other.X - X) + Math.Abs(other.Y + Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other.GetType() != typeof(Vector))
                return false;

            return ((Vector) other).X == X && ((Vector) other).Y == Y;
        }
    }

    private static readonly Vector North = new Vector(0, 1);
    private static readonly Vector East = new Vector(1, 0);
    private static readonly Vector South = new Vector(0, -1);
    private static readonly Vector West = new Vector(-1, 0);

    static Vector TurnVector(Vector currentDirection, char turn)
    {
        switch (turn)
        {
            case 'R':
                if (currentDirection.Equals(North))
                    return East;
                if (currentDirection.Equals(East))
                    return South;
                if (currentDirection.Equals(South))
                    return West;
                return North;

            case 'L':
                if (currentDirection.Equals(North))
                    return West;
                if (currentDirection.Equals(West))
                    return South;
                if (currentDirection.Equals(South))
                    return East;
                return North;

            default:
                throw new ArgumentOutOfRangeException(nameof(turn), turn, "Must be R or L");
        }
    }


    private static IEnumerable<Vector> Walk(Vector start, Vector end)
    {
        if (start.X != end.X)
        {
            var sign = start.X < end.X ? 1 : -1;
            return Enumerable.Range(1, Math.Abs(end.X - start.X)).Select(x => new Vector(start.X + x * sign, start.Y));
        }
        else
        {
            var sign = start.Y < end.Y ? 1 : -1;
            return Enumerable.Range(1, Math.Abs(end.Y - start.Y)).Select(y => new Vector(start.X, start.Y + y * sign));
        }
    }

    static void Main()
    {
        var result =
            "R3, L2, L2, R4, L1, R2, R3, R4, L2, R4, L2, L5, L1, R5, R2, R2, L1, R4, R1, L5, L3, R4, R3, R1, L1, L5, L4, L2, R5, L3, L4, R3, R1, L3, R1, L3, R3, L4, R2, R5, L190, R2, L3, R47, R4, L3, R78, L1, R3, R190, R4, L3, R4, R2, R5, R3, R4, R3, L1, L4, R3, L4, R1, L4, L5, R3, L3, L4, R1, R2, L4, L3, R3, R3, L2, L5, R1, L4, L1, R5, L5, R1, R5, L4, R2, L2, R1, L5, L4, R4, R4, R3, R2, R3, L1, R4, R5, L2, L5, L4, L1, R4, L4, R4, L4, R1, R5, L1, R1, L5, R5, R1, R1, L3, L1, R4, L1, L4, L4, L3, R1, R4, R1, R1, R2, L5, L2, R4, L1, R3, L5, L2, R5, L4, R5, L5, R3, R4, L3, L3, L2, R2, L5, L5, R3, R4, R3, R4, R3, R1"
                .Split(new[] {", "}, StringSplitOptions.None)
                .Select(pair => new {TurnDirection = pair[0], Distance = int.Parse(pair.Substring(1))})
                .Aggregate(
                    new
                    {
                        Visited = new HashSet<Vector>(new[] {new Vector(0, 0)}),
                        Position = new Vector(0, 0),
                        Direction = North,
                        FirstCrossing = new Vector()
                    },
                    (state, instruction) =>
                    {
                        var newDirection = TurnVector(state.Direction, instruction.TurnDirection);

                        var newPosition = new Vector
                        (
                            state.Position.X + newDirection.X*instruction.Distance,
                            state.Position.Y + newDirection.Y*instruction.Distance
                        );

                        var walk = Walk(state.Position, newPosition).ToArray();

                        var newFirstCrossing = walk.FirstOrDefault(state.Visited.Contains);

                        return new
                        {
                            Visited = new HashSet<Vector>(state.Visited.Concat(walk)),
                            Position = newPosition,
                            Direction = newDirection,
                            FirstCrossing = state.FirstCrossing.IsValid ? state.FirstCrossing : newFirstCrossing
                        };
                    }
                );

        Console.WriteLine(new Vector(0, 0).DistanceTo(result.Position));
        Console.WriteLine(new Vector(0, 0).DistanceTo(result.FirstCrossing));
    }
}
