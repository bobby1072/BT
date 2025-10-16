using System.Net;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public sealed class ApiClientException: ApiException
{
    public ApiClientException(HttpStatusCode httpStatusCode, string message): base(LogLevel.Information, httpStatusCode, message) { }
    public ApiClientException(LogLevel logLevel, HttpStatusCode statusCode, string message) : base(logLevel, statusCode, message) { }
}