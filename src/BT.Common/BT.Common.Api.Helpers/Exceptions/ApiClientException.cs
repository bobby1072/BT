using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public sealed class ApiClientException: ApiException
{
    public ApiClientException(HttpStatusCode httpStatusCode, string message, Exception? innerException = null): base(LogLevel.Information, httpStatusCode, message, innerException) { }
    public ApiClientException(LogLevel logLevel, HttpStatusCode statusCode, string message, Exception? innerException = null) : base(logLevel, statusCode, message, innerException) { }
}