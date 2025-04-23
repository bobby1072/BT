using System.Text.Json;

namespace BT.Common.Helpers.Extensions;

public static class DictionaryExtensions
{
    public static string? SerialiseToJson<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary,
        JsonSerializerOptions? options = null) where TKey : notnull
    {
        return dictionary == null ? null : JsonSerializer.Serialize(dictionary, options);
    }
    
    
    public static bool IsStringSequenceEqual(this Dictionary<string, string>? dict, Dictionary<string, string>? otherDict)
    {
        if (dict == null || otherDict == null)
        {
            if(dict == null && otherDict == null) return true;
            return false;
        }
        if (dict?.Count != otherDict?.Count) return false;
        foreach (var mainObj in dict ?? [])
        {
            if (otherDict?.TryGetValue(mainObj.Key, out var value) != true) return false;
            if (value?.Equals(mainObj.Value) != true) return false;
        }

        return true;
    }

    public static void TryOverrideAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue val) where TKey : notnull
    {
        try
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = val;
                return;
            }
            dict.Add(key, val);
        }
        catch
        {
            //No need to handle    
        }
    }
    public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dict) where TKey : notnull
    {
        return dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}