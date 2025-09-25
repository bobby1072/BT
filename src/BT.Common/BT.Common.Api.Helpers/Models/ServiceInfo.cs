namespace BT.Common.Api.Helpers.Models;

public sealed record ServiceInfo
{
    public static readonly string Key = nameof(ServiceInfo);
    
    public required string ReleaseName { get; init; }
    
    public required string ReleaseVersion { get; init; }
    
    public Guid ApplicationInstanceId { get; } = Guid.NewGuid();
}