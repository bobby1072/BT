namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static IEnumerable<T> FastArrayWhere<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            foreach (var value in values)
            {
                if (predicate.Invoke(value))
                {
                    yield return value;
                }
            }
        }
    }
}