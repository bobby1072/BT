namespace BT.Common.Api.Helpers.Models;

public sealed record HealthResponse
{
    public required ServiceInfo ServiceInfo { get; init; }
    public DateTime CurrentLocalTime => DateTime.Now.ToLocalTime();
}