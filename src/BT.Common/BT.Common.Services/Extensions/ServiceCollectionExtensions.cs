using System.Diagnostics;
using BT.Common.Services.Abstract;
using BT.Common.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Common.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelemetryService(this IServiceCollection services, string activitySourceName)
    {
        TelemetryHelperService.SetActivitySource(new ActivitySource(activitySourceName));
        
        services
            .AddOpenTelemetry()
            .WithTracing(ctx =>
            {
                ctx.AddSource(activitySourceName);
            });
        
        
        return services;
    }
    
    public static IServiceCollection AddDistributedCachingService(this IServiceCollection services)
    {
        services
            .AddDistributedMemoryCache()
            .AddSingleton<ICachingService, DistributedCachingService>();
        
        return services;
    }
}