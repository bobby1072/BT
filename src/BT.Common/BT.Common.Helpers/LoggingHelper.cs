using Microsoft.Extensions.Logging;

namespace BT.Common.Helpers;

public static class LoggingHelper
{
    public static readonly ILogger StaticLogger = new LoggerFactory().CreateLogger<object>();
}