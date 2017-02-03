using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AoC2016_22
{
    internal class AoCParser<T> where T: IEquatable<T>, new()
    {
        private Regex Expression { get; }
        public AoCParser(Regex expression)
        {
            Expression = expression;
        }
        public IEnumerable<T> Parse(string input)
        {
            return input
                .Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseLine)
                .Where(i => i != null);
        }
        public T ParseLine(string line)
        {
            var match = Expression.Match(line);

            if (!match.Success)
            {
                Console.WriteLine("Couldn't match '" + line + "'");
                return default(T);
            }

            var obj = new T();

            foreach (var name in Expression.GetGroupNames().Where(name => name != "0"))
            {
                var value = match.Groups[name].Captures[0].Value;

                var propertyType = typeof(T).GetProperty(name).PropertyType;

                typeof(T).InvokeMember
                (
                    name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                    Type.DefaultBinder,
                    obj,
                    new[]{Convert.ChangeType(value, propertyType)}
                );
            }

            return obj;
        }
    }
}
