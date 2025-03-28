using System.Text.Json;

namespace BT.Common.Helpers;

public static class DictionaryHelpers
{
    public static Dictionary<TKey, TValue> DeserialiseFromJsonString<TKey, TValue>(string? stringJson)
    {
        return string.IsNullOrEmpty(stringJson) ? null: JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(stringJson);
    }
}