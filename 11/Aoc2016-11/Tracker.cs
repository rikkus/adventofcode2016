namespace Aoc2016_11
{
    internal class Tracker
    {
        public State State { get; set; }
        public Tracker Previous { get; set; }
        public int Depth { get; set; }
    }
}