using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2016_11
{
    internal class State : IComparable<State>
    {
        public int Elevator { get; set; }
        public List<List<Actor>> Floors { get; set; }

        public int CompareTo(State other)
        {
            return string.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            var ret = new StringBuilder();

            var allActors = Floors.SelectMany(f => f.Select(a => a)).OrderBy(a => a.Description).ToArray();
            
            for (var i = Floors.Count - 1; i >= 0; i--)
            {
                var elevatorSymbol = Elevator == i ? "E" : ".";
                var floor = Floors[i];
                ret.Append($"F{i + 1} {elevatorSymbol} ");
                foreach (var actor in allActors)
                {
                    if (floor.Contains(actor))
                        ret.Append(actor + " ");
                    else
                        ret.Append(".   ");
                }
                ret.AppendLine();
            }

            return ret.ToString();
        }

        public static State Parse(string input)
        {
            var floorDescriptions = input.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseFloor)
                .ToDictionary(d => d.Item1, d => d.Item2);

            const int floorCount = 4;

            var floors = Enumerable.Repeat(new List<Actor>(), floorCount).ToList();

            foreach (var desc in floorDescriptions)
            {
                floors[desc.Key] = desc.Value.ToList();
            }

            return new State {Elevator = 0, Floors = floors};
        }

        private static readonly string[] Ordinals = {"first", "second", "third", "fourth"};

        private static readonly Regex ChipRegex = new Regex("(?<element>[a-z]+)-compatible microchip");
        private static readonly Regex GeneratorRegex = new Regex("(?<element>[a-z]+) generator");
        private static readonly Regex FloorRegex = new Regex("^The (?<nth>[a-z]+) floor");

        private static Tuple<int, IEnumerable<Actor>> ParseFloor(string floorDescription)
        {
            var floor = Ordinals.ToList().IndexOf(FloorRegex.Match(floorDescription).Groups["nth"].Value);

            var actors =
                GeneratorRegex
                    .Matches(floorDescription)
                    .Cast<Match>()
                    .Select(m => m.Groups["element"].Value)
                    .Select(element => new Actor(element, chip: false))
                    .Union
                    (
                        ChipRegex
                            .Matches(floorDescription)
                            .Cast<Match>()
                            .Select(m => m.Groups["element"].Value)
                            .Select(element => new Actor(element, chip: true))
                    );

            return Tuple.Create(floor, actors);
        }
    }
}