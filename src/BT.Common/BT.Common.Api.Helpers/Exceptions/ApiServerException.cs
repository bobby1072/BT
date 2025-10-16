using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public sealed class ApiServerException: ApiException
{
    public ApiServerException(string message) : base(LogLevel.Error, HttpStatusCode.InternalServerError, message) { }
    public ApiServerException(HttpStatusCode statusCode, string message) : base(LogLevel.Error, statusCode, message) { }
    public ApiServerException(LogLevel logLevel, HttpStatusCode statusCode, string message) : base(logLevel, statusCode, message) { }
}