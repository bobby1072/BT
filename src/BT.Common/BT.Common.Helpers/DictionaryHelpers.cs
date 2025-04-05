using System.Text.Json;

namespace BT.Common.Helpers;

public static class DictionaryHelpers
{
    public static Dictionary<TKey, TValue>? DeserializeFromJsonString<TKey, TValue>(string? stringJson) where TKey: notnull
    {
        return string.IsNullOrEmpty(stringJson) ? null: JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(stringJson);
    }
}