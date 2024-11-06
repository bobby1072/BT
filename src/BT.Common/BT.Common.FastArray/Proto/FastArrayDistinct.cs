namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static IEnumerable<T> FastArrayDistinct<T>(this IEnumerable<T> values)
        {
            var hashSet = new HashSet<T>();
            foreach (var value in values)
            {
                if (hashSet.Add(value))
                {
                    yield return value;
                }
            }
        }
        public static IEnumerable<T> FastArrayDistinctBy<T, TKey>(this IEnumerable<T> values, Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var value in values)
            {
                var key = keySelector.Invoke(value);
                if (seenKeys.Add(key))
                {
                    yield return value;
                }
            }
        }
    }
}