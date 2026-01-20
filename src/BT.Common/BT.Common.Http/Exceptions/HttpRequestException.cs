using System.Net;

namespace BT.Common.Http.Exceptions;

public sealed class HttpRequestException: Exception
{
    public HttpStatusCode? HttpStatusCode { get; set; }

    public HttpRequestException(string? message, HttpStatusCode? statusCode = null) : base(message)
    {
        HttpStatusCode = statusCode;
    }

    public HttpRequestException(string? message, HttpStatusCode? statusCode, Exception? innerException) : base(
        message, innerException)
    {
        HttpStatusCode = statusCode;
    }
}