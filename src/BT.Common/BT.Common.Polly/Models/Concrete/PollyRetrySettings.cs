using BT.Common.Polly.Models.Abstract;

namespace BT.Common.Polly.Models.Concrete;

public record PollyRetrySettings : IPollyRetrySettings
{
    public int? TimeoutInSeconds { get; init; }
    public int? TotalAttempts { get; init; }
    public int? DelayBetweenAttemptsInSeconds { get; init; }
    public bool? UseJitter { get; init; }
}