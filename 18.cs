using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2016_18
{
    class Program
    {
        static void Main()
        {
            var testInput = @".^^.^.^^^^";
            var testCount = 10;

            var realInput =
                @"...^^^^^..^...^...^^^^^^...^.^^^.^.^.^^.^^^.....^.^^^...^^^^^^.....^.^^...^^^^^...^.^^^.^^......^^^^";
            var realCount = 40;

            var input = realInput;
            var count = realCount;

            var ret = new List<string> {input};

            for (var row = 0; row < count - 1; row++)
            {
                var virtualRow = "." + input + ".";
                var newRow = new StringBuilder(input.Length);

                for (int i = 1; i <= input.Length; i++)
                {
                    newRow.Append(Calc(virtualRow[i - 1], virtualRow[i], virtualRow[i + 1]));
                }

                Console.WriteLine(newRow.ToString());
                ret.Add(newRow.ToString());

                input = newRow.ToString();
            }

            Console.WriteLine(ret.SelectMany(s => s.ToCharArray()).Count(c => c.Equals('.')));

            Console.ReadKey();

            //1982?
        }

        static char Calc(char a, char b, char c)
        {
            if (a == '^' && b == '^' && c == '.')
                return '^';

            if (a == '.' && b == '^' && c == '^')
                return '^';

            if (a == '^' && b == '.' && c == '.')
                return '^';

            if (a == '.' && b == '.' && c == '^')
                return '^';

            return '.';
        }
    }
}
