using BT.Common.HttpClient.Models;
using Polly;
using Polly.Retry;

namespace BT.Common.HttpClient.Extensions;

public static class PollyRetrySettingsExtensions
{
    internal static ResiliencePipeline ToPipeline(this PollyRetrySettings pollyRetrySettings)
    {
        var pipeline = new ResiliencePipelineBuilder();
        if (pollyRetrySettings.TimeoutInSeconds > 0)
        {
            pipeline.AddTimeout(TimeSpan.FromSeconds((double)pollyRetrySettings.TimeoutInSeconds));
        }

        if (pollyRetrySettings.TotalAttempts > 1
            || pollyRetrySettings.UseJitter is not null
            || pollyRetrySettings.DelayBetweenAttemptsInSeconds is not null)
        {
            var retryOptions = new RetryStrategyOptions();

            if (pollyRetrySettings.TotalAttempts > 1)
            {
                retryOptions.MaxRetryAttempts = (int)pollyRetrySettings.TotalAttempts! - 1;
            }
            if (pollyRetrySettings.UseJitter is not null)
            {
                retryOptions.UseJitter = retryOptions.UseJitter;
            }
            if (pollyRetrySettings.DelayBetweenAttemptsInSeconds is not null)
            {
                retryOptions.Delay = TimeSpan.FromSeconds((double)pollyRetrySettings.DelayBetweenAttemptsInSeconds);
            }
            pipeline.AddRetry(retryOptions);
        }
        
        return pipeline.Build(); 
    }
}