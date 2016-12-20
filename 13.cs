using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2016_14
{
    internal class Program
    {
        private readonly Dictionary<int, string> Hashes = new Dictionary<int, string>(1000);
        private readonly Dictionary<Tuple<char, int>, bool> Matches = new Dictionary<Tuple<char, int>, bool>();

        static void Main()
        {
            new Program().Compute();
        }

        public void Compute()
        { 
            const string testInput = "abc";
            const string realInput = "zpqevtbw";

            var index = 0;
            var keyCount = 0;
            var salt = realInput;

            while (keyCount < 64)
            {
                if (IsKey(salt, index++))
                    keyCount++;
            }

            Console.WriteLine(index - 1);
            Console.ReadKey();

            // 16106?
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);

            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        string Hash(string salt, int index)
        {
            string hash;

            if (Hashes.TryGetValue(index, out hash))
            {
                return hash;
            }

            Hashes[index]
                = hash
                    = ByteArrayToString
                    (
                        System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(salt + index))
                    );
                

            if (Hashes.ContainsKey(index - 1000))
                Hashes.Remove(index - 1000);

            return hash;
        }

        private static readonly Regex ThreeInARow = new Regex(@"(.)\1\1", RegexOptions.Compiled);
        private readonly Dictionary<char, Regex> fiveInARowDictionary = new Dictionary<char, Regex>();

        private Regex FiveInARow(char c)
        {
            Regex regex;

            if (fiveInARowDictionary.TryGetValue(c, out regex))
            {
                return regex;
            }

            regex = new Regex("(" + c + @")\1\1\1\1");

            fiveInARowDictionary[c] = regex;

            return regex;
        }

        bool IsKey(string salt, int index)
        {
            var hash = Hash(salt, index);

            var match = ThreeInARow.Match(hash);

            if (!match.Success)
                return false;
            
            var c = match.Value[0];

            var fiveInARow = FiveInARow(c);

            for (var i = index + 1; i < index + 1002; i++)
            {
                bool isMatch;
                var id = Tuple.Create(c, i);

                if (Matches.TryGetValue(id, out isMatch) && isMatch)
                {
                    //Console.WriteLine($"Full match on {c} at {i}");

                    return true;
                }

                bool matchedFive = fiveInARow.IsMatch(Hash(salt, i));

                Matches[id] = matchedFive;

                if (matchedFive)
                {
                    //Console.WriteLine($"Full match on {c} at {i}");

                    return true;
                }
            }

            return false;
        }
    }
}
