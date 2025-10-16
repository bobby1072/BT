using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public class ApiException: Exception
{
    public LogLevel LogLevel { get; }
    public HttpStatusCode StatusCode { get; }

    public ApiException(LogLevel logLevel, HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
    }
}