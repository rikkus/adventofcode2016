using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2016_11
{
    internal class State
    {
        public int Elevator { get; }
        public List<List<Actor>> Floors { get; }

        private string _description;
        private int? _hashCode;

        public State(int elevator, List<List<Actor>> floors)
        {
            Elevator = elevator;
            Floors = floors;
        }

        public override string ToString()
        {
            return _description ?? (_description = Description());
        }

        public override int GetHashCode()
        {
            if (_hashCode.HasValue)
                return _hashCode.Value;

            int n = 0;
            var ret = new StringBuilder();
            foreach (var floor in Floors)
            {
                ret.Append(n == Elevator ? "!" : "-");
                foreach (var actor in floor.OrderBy(a => a))
                {
                    ret.Append(actor);
                }
                ++n;
            }

            _hashCode = ret.ToString().GetHashCode();
            return _hashCode.Value;
        }

        private string Description()
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

            return new State(0, floors);
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