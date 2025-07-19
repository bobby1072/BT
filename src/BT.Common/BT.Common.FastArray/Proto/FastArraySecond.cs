namespace BT.Common.FastArray.Proto;

public static partial class FastArray
{
    public static T? FastArraySecond<T>(this IEnumerable<T> values, Func<T, bool>? predicate = null)
    {
        var newArray = values.Skip(1).ToArray();

        return newArray.First();
    }    
    public static T? FastArraySecondOrDefault<T>(this IEnumerable<T> values, Func<T, bool>? predicate = null)
    {
        var newArray = values.Skip(1).ToArray();

        return newArray.FirstOrDefault();
    }    
}