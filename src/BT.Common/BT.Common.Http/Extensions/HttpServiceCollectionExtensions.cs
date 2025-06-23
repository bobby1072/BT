using BT.Common.Polly.Models.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace BT.Common.Http.Extensions;

public static class HttpServiceCollectionExtensions
{
    private const string _resiliencePipelinePrefix = "resilience-pipeline-";
    
    
    public static IServiceCollection AddHttpClientWithResilience<TService, TImplementation>(this IServiceCollection services, IPollyRetrySettings pollyRetrySettings)
        where TService : class
        where TImplementation : class, TService
    {
        var numberOfRetries = (pollyRetrySettings.TotalAttempts ?? 0) - 1 > 0 ? (pollyRetrySettings.TotalAttempts ?? 0) - 1: 0;
        var delay = TimeSpan.FromSeconds((pollyRetrySettings.DelayBetweenAttemptsInSeconds ?? 0) > 0
            ? pollyRetrySettings.DelayBetweenAttemptsInSeconds ?? 0
            : 0);
        
        services
            .AddHttpClient<TService, TImplementation>()
            .AddResilienceHandler($"{_resiliencePipelinePrefix}{typeof(TService).Name}", x =>
            {
                x.AddRetry(new HttpRetryStrategyOptions
                {
                    UseJitter = pollyRetrySettings.UseJitter ?? false,
                    MaxRetryAttempts = numberOfRetries,
                    Delay = delay,
                    BackoffType = DelayBackoffType.Constant
                });

                if (pollyRetrySettings.TimeoutInSeconds is not null)
                {
                    x.AddTimeout(TimeSpan.FromSeconds(pollyRetrySettings.TimeoutInSeconds.Value));
                }
            });
        
        
        return services;
    }
}