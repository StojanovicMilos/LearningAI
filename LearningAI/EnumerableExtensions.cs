using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningAI
{
    public static class EnumerableExtensions
    {
        public static T WithMaximum<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> criteria)
            where T : class
            where TKey : IComparable<TKey> =>
            sequence.Aggregate((T)null, (best, current) =>
                best == null || criteria(current).CompareTo(criteria(best)) > 0 ? current : best);
    }
}