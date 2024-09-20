﻿namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static IEnumerable<TNew> FastArraySelect<TOriginal, TNew>(this IEnumerable<TOriginal> values, Func<TOriginal, TNew> transformFunc)
        {
            foreach (var value in values)
            {
                yield return transformFunc.Invoke(value);
            }
        }
        public static IEnumerable<TNew> FastArraySelectWhere<TOriginal, TNew>(this IEnumerable<TOriginal> values, Func<TOriginal, bool> predicate, Func<TOriginal, TNew> transformFunc)
        {
            foreach (var value in values)
            {
                if (predicate.Invoke(value))
                {
                    yield return transformFunc.Invoke(value);
                }
            }
        }
    }
}
