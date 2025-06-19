using System.Text.Json;

namespace BT.Common.FastArray.Proto
{
    public static partial class FastArray
    {
        public static T FastArrayFirst<T>(this IEnumerable<T> values, Func<T, bool>? predicate = null)
        {
            return values.FastArrayFirstOrDefault(predicate) ?? throw new NullReferenceException();
        }
        
        
        public static T? FastArrayFirstOrDefault<T>(this IEnumerable<T> values, Func<T, bool>? predicate = null)
        {
            foreach (var value in values)
            {
                if (predicate is null || predicate.Invoke(value))
                {
                    return value;
                }
            }
            return default;
        }
    }
}