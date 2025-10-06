using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace BT.Common.Helpers;

public static class LoggingHelper
{
    public static IServiceCollection AddJsonLogging(this IServiceCollection services)
    {
        services.AddLogging(opts =>
        {
            opts.ClearProviders();
            opts.AddJsonConsole(ctx =>
            {
                ctx.IncludeScopes = true;
                ctx.UseUtcTimestamp = true;
            });
        });
        
        return services;
    }
    
    public static ILogger CreateLogger()
    {
        using var loggerFactory = LoggerFactory.Create(logBuilder =>
            logBuilder.AddJsonConsole(ConfigureJsonConsole)
        );

        var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();

        return logger;
    }

    private static void ConfigureJsonConsole(JsonConsoleFormatterOptions options)
    {
        options.IncludeScopes = true;
        options.UseUtcTimestamp = true;
    }
}
