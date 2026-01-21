using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace BT.Common.Helpers;

public static class LoggingHelper
{
    public static ILoggingBuilder AddJsonLogging(this ILoggingBuilder builder,  Action<JsonConsoleFormatterOptions>? optionAction = null)
    {
        builder.ClearProviders();
        builder.AddJsonConsole(x => ConfigureJsonConsole(x, optionAction));
        
        return builder;
    }
    
    public static ILogger CreateLogger()
    {
        using var loggerFactory = LoggerFactory.Create(logBuilder =>
            logBuilder.AddJsonConsole(x => ConfigureJsonConsole(x))
        );

        var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();

        return logger;
    }

    private static void ConfigureJsonConsole(JsonConsoleFormatterOptions options, Action<JsonConsoleFormatterOptions>? optionAction = null)
    {
        if (optionAction is not null)
        {
            optionAction.Invoke(options);
        }
        else
        {
            options.IncludeScopes = true;
            options.UseUtcTimestamp = true;
        }
    }
}
