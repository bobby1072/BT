using System.Runtime.Serialization;
using System.Text.Json;
using Flurl.Http.Configuration;

namespace BT.Common.Http.Serializers;

public class DefaultFlurlJsonSerializer: ISerializer
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions = null;

    public DefaultFlurlJsonSerializer(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }
    
    
    
    public string Serialize(object obj) => JsonSerializer.Serialize(obj, _jsonSerializerOptions);
    
    public T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions)
        ?? throw new SerializationException($"Unable to deserialize response to {typeof(T).Name}");
    
    public T Deserialize<T>(Stream bytes) => JsonSerializer.Deserialize<T>(bytes, _jsonSerializerOptions)
        ?? throw new SerializationException($"Unable to deserialize response to {typeof(T).Name}");
}