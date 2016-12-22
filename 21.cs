using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC2016_21
{
   public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), batchSize, "Must be > 0");

            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return Enumerate(enumerator, batchSize);
                }
            }
        }

        private static IEnumerable<T> Enumerate<T>(IEnumerator<T> source, int size)
        {
            var i = 0;

            do {
                yield return source.Current;
            } while (++i < size && source.MoveNext());
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Func<int[], bool> IsValidTriangle = (sides) => (sides.Sum() - sides.Max()) > sides.Max();

            var possibleCount = Constants.Input
               .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
               .Select(row => row.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
               .Select(row => new[] { int.Parse(row[0]), int.Parse(row[1]), int.Parse(row[2]) })
               .Batch(3)
               .Sum(batch =>
                   {
                       var a = batch.ToArray();
                       return new[] {
                                new[] { a[0][0], a[1][0], a[2][0] },
                                new[] { a[0][1], a[1][1], a[2][1] },
                                new[] { a[0][2], a[1][2], a[2][2] }
                       }.Count(IsValidTriangle);
                   }
               );

            Console.WriteLine(possibleCount);
            Console.ReadKey();

            // 1577?
        }
    }
}
