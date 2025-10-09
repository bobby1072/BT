using System.Text.Json;

namespace BT.Common.Helpers.Models;

public sealed class LoggingScopeVariableDictionary: Dictionary<string, object>
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}