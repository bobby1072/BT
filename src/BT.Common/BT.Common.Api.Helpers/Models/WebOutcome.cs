namespace BT.Common.Api.Helpers.Models;

public record WebOutcome
{
    public string? ExceptionMessage { get; init; }
    public Dictionary<string, object> ExtraData { get; init; } = new();
    public bool IsSuccess => string.IsNullOrEmpty(ExceptionMessage);
}

public record WebOutcome<T>: WebOutcome
{
    public T? Data { get; init; }
}