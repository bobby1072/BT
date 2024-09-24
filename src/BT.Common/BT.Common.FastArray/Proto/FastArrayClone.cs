namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static IEnumerable<T> FastArrayClone<T>(this IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }
}