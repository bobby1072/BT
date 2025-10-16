using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public sealed class ApiServerException: ApiException
{
    public ApiServerException(string message, Exception? innerException = null) : base(LogLevel.Error, HttpStatusCode.InternalServerError, message, innerException) { }
    public ApiServerException(HttpStatusCode statusCode, string message, Exception? innerException = null) : base(LogLevel.Error, statusCode, message, innerException) { }
    public ApiServerException(LogLevel logLevel, HttpStatusCode statusCode, string message, Exception? innerException = null) : base(logLevel, statusCode, message, innerException) { }
}