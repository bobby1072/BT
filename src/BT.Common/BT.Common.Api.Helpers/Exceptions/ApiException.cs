using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public class ApiException: Exception
{
    public LogLevel LogLevel { get; }
    public HttpStatusCode StatusCode { get; }
    public Dictionary<string, object?> ExtraData { get; set; } = new();
    public ApiException(LogLevel logLevel, HttpStatusCode statusCode, string message, Exception? innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
    }
}