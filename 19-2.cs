using System;

namespace AoC2016_19
{
    class Elf
    {
        public Elf(string name)
        {
            Name = name;
            Value = 1;
            Previous = this;
            Next = this;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public int Value { get; set; }
        public Elf Previous { get; set; }
        public Elf Next { get; set; }
    }
    public class ElfCircle
    {
        private int count;
        public void Dump()
        {
            var currentElf = elf1;

            Console.Write(currentElf);
            if (currentElf.Next != currentElf)
            {
                while (currentElf != elf2)
                {
                    currentElf = currentElf.Next;
                    Console.Write(" " + currentElf);
                }
                while (currentElf != elf1)
                {
                    currentElf = currentElf.Next;
                    Console.Write(" " + currentElf);
                }
            }
            Console.WriteLine();
        }

        private Elf elf1 = new Elf("1");
        private Elf elf2;

        public ElfCircle(int elfCount)
        {
            if (elfCount == 0)
                throw new ArgumentOutOfRangeException(nameof(elfCount), elfCount, "Must be > 0");

            count = elfCount;

            if (elfCount == 1)
            {
                return;
            }

            var currentElf = elf1;

            for (int i = 2; i <= elfCount; i++)
            {
                currentElf.Next = new Elf(i.ToString()) {Previous = currentElf};
                currentElf = currentElf.Next;
                if (i == elfCount/2 + 1)
                    elf2 = currentElf;
            }

            currentElf.Next = elf1;
            elf1.Previous = currentElf;
        }

        public string Reduce()
        {
            while (elf1.Next != elf1)
            {
                elf1.Value += elf2.Value;
                elf2.Value = 0;

                if (elf2.Value == 0)
                {
                    elf2.Previous.Next = elf2.Next;
                    elf2.Next.Previous = elf2.Previous;
                    elf2 = elf2.Next;
                    --count;

                    if (count%2 == 0)
                        elf2 = elf2.Next;
                }

                if (elf1.Next != elf1)
                    elf1 = elf1.Next;
            }

            return elf1.Name;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var testInput = 5;
            var realInput = 3004953;

            var circle = new ElfCircle(realInput);
            //circle.Dump();
            Console.WriteLine(circle.Reduce());
            Console.ReadKey();
        }
    }
}
