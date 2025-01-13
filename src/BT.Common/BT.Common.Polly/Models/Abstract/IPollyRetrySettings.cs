namespace BT.Common.Polly.Models.Abstract
{
    public interface IPollyRetrySettings
    {
        int? TimeoutInSeconds { get; }
        int? TotalAttempts { get; }
        int? DelayBetweenAttemptsInSeconds { get; }
        bool? UseJitter { get; }
    }
}
