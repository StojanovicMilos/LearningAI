using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningAI
{
    public static class EnumerableExtensions
    {
        public static T WithMaximumScore<T>(this IEnumerable<T> sequence)
            where T : Dot =>
            sequence.Aggregate((T)null, (best, current) =>
                best == null || current.Fitness.Score()>best.Fitness.Score() ? current : best);
    }
}