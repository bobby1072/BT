namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static T? FastArrayFirstOrDefault<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            foreach (var value in values)
            {
                if (predicate.Invoke(value))
                {
                    return value;
                }
            }
            return default;
        }
    }
}