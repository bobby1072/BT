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
    }
}