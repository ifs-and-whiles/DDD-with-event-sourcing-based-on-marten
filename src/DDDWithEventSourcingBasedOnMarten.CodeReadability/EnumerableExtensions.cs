using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Billy.CodeReadability
{
    public static class EnumerableExtensions
    {
        public static TItem Max<TCollection, TItem>(
            this IEnumerable<TCollection> enumerable,
            Func<TCollection, TItem> itemSelectorFunc,
            Func<TCollection, double> valueToCompareSelectorFunc)
        {
            double? maxValue = null;
            var maxItem = default(TItem);

            foreach (var item in enumerable)
            {
                var value = valueToCompareSelectorFunc(item);

                if (maxValue == null || value > maxValue)
                {
                    maxValue = value;
                    maxItem = itemSelectorFunc(item);
                }
            }

            return maxItem;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
