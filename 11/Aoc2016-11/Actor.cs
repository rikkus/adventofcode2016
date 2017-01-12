using System;
using System.Linq;

namespace Aoc2016_11
{
    internal class Actor
    {
        public string Element => Description.Substring(0, 2);

        public bool IsChip => Description[2] == 'M';
        public bool IsGenerator => Description[2] == 'G';

        public string Description { get; }

        public Actor(string element, bool chip)
        {
            Description = new string(element.ToUpperInvariant().Take(2).Concat(new[] { chip ? 'M' : 'G' }).ToArray());
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Actor))
                throw new ArgumentException("I only compare Actors", nameof(obj));

            return string.Compare(((Actor)obj).Description, Description, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}