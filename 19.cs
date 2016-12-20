using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2016_19
{
    class Elf
    {
        public int Index { get; set; }
        public int Value { get; set; }
        public Elf Next { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var testInput = 5;
            var realInput = 3004953;

            var input = realInput;

            var elves = Enumerable.Range(0, input - 1)
                .Aggregate(new List<Elf> {new Elf {Index = 1, Value = 1, Next = null}},
                    (list, i) =>
                    {
                        var previousElf = list.Last();

                        var nextElf = new Elf {Index = previousElf.Index + 1, Value = 1, Next = null};

                        list.Last().Next = nextElf;

                        list.Add(nextElf);

                        return list;
                    }
                );

            elves.Last().Next = elves.First();

            var currentElf = elves.First();

            while (currentElf.Value != elves.Count)
            {
                //Console.WriteLine($"Elf {currentElf.Index} takes {currentElf.Next.Value} from {currentElf.Next.Index} and links to {currentElf.Next.Next.Index}");
                currentElf.Value += currentElf.Next.Value;
                currentElf.Next = currentElf.Next.Next;
                currentElf = currentElf.Next;
            }

            Console.WriteLine(currentElf.Index);
            Console.ReadKey();
        }
    }
}
