using System;
using System.Collections.Generic;
using System.Linq;

namespace Billy.CodeReadability
{
    public static class OptionExtensions
    {
        public static IEnumerable<T> SelectOptionItems<T>(this IEnumerable<Option<T>> options)
        {
            return options.SelectMany(option => option.ToEnumerable());
        }

        public static Option<string> Add(this Option<string> first, Option<string> second)
        {
            var items = new[] {first, second}
                .SelectOptionItems()
                .ToArray();

            return items.Any()
                ? items.Aggregate("", (acc, value) => acc + value)
                : Option<string>.None;
        }
    }
}