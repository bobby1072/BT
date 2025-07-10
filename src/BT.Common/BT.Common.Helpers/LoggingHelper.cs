using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace BT.Common.Helpers;

public static class LoggingHelper
{
    public static ILogger CreateLogger()
    {
        using var loggerFactory = LoggerFactory.Create(logBuilder=> logBuilder.AddJsonConsole(ConfigureJsonConsole));
        
        var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();

        return logger;
    }
    
    private static void ConfigureJsonConsole(JsonConsoleFormatterOptions options)
    {
        options.IncludeScopes = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
        options.UseUtcTimestamp = true;
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions
        {
            Indented = false,
        };
    }
}