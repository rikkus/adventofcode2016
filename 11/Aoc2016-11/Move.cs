using System.Collections.Generic;

namespace Aoc2016_11
{
    public enum Direction
    {
        Up,
        Down
    }

    class Move
    {
        public Direction Direction { get; set; }
        public IEnumerable<Actor> Actors { get; set; }

        public override string ToString()
        {
            return $"{Direction} {string.Join(", ", Actors)}";
        }
    }
}