using BT.Common.Polly.Models.Concrete;

namespace BT.Common.Persistence.Shared.Configurations;

public sealed record DbMigrationSettings : PollyRetrySettings
{
    public static readonly string Key = nameof(DbMigrationSettings);
    public required string StartVersion { get; init; }
}
