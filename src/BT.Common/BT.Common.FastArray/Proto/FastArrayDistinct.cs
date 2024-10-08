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
        public static IEnumerable<T> FastArrayDistinctBy<T, TKey>(this IEnumerable<T> values, Func<T, TKey> keySelector, Func<TKey, TKey, bool>? comparer = null)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var value in values)
            {
                var key = keySelector.Invoke(value);
                if (comparer == null)
                {
                    if (seenKeys.Add(key))
                    {
                        yield return value;
                    }
                }
                else
                {
                    var found = false;
                    foreach (var seenKey in seenKeys)
                    {
                        if (comparer.Invoke(seenKey, key))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        seenKeys.Add(key);
                        yield return value;
                    }
                }
            }
        }
    }
}