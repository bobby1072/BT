namespace BT.Common.HttpClient.Models;

public record PollyRetrySettings
{
    public int? TimeoutInSeconds { get; init; }
    public int? TotalAttempts { get; init; }
    public int? DelayBetweenAttemptsInSeconds { get; init; }
    public bool? UseJitter { get; init; }
}