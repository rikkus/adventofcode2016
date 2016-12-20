using System;
using System.Linq;
using System.Text;

namespace Aoc2016_16
{
    class Program
    {
        static void Main(string[] args)
        {
            var testInput = "110010110100";
            var testLength = 12;

            var realInput = "11101000110010100";
            var realLength = 272;

            // I am very sorry for doing this with strings rather than bits. Sorry.
            
            var disk = Fill(realInput, realLength);

            Console.WriteLine($"Size on disk: {disk.Length}");
            var checksum = Checksum(disk);

            Console.WriteLine(checksum);
            Console.ReadKey();

            // 10100101010101101?
        }

        private static string ChecksumInternal(string s)
        {
            var checksum = new StringBuilder();

            for (int i = 0; i < s.Length / 2; i++)
            {
                var pair = s.Substring(i * 2, 2);

                checksum.Append(pair[0] == pair[1] ? '1' : '0');
            }

            return checksum.ToString();
        }

        private static string Checksum(string s)
        {
            string checksum = ChecksumInternal(s);

            while (checksum.Length%2 == 0)
            {
                checksum = ChecksumInternal(checksum);
            }

            return checksum;
        }

        private static string Fill(string s, int desiredLength)
        {
            var ret = s;

            while (ret.Length < desiredLength)
            {
                ret = DragonCurve(ret);
            }

            return ret.Substring(0, desiredLength);
        }

        private static string DragonCurve(string s)
        {
            return s + '0' + new string(s.Replace('1', '2').Replace('0', '1').Replace('2', '0').Reverse().ToArray());
        }
    }
}
