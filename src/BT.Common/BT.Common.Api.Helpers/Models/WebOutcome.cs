namespace BT.Common.Api.Helpers.Models;

public record WebOutcome
{
    public string? ExceptionMessage { get; init; }
    public Dictionary<string, object>? ExtraData { get; init; }
    public bool IsSuccess => string.IsNullOrEmpty(ExceptionMessage);
}

public sealed record WebOutcome<T>: WebOutcome
{
    public T? Data { get; init; }
}