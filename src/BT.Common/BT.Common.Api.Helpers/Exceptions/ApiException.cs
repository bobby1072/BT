using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Exceptions;

public class ApiException: Exception
{
    public LogLevel LogLevel { get; }
    public HttpStatusCode StatusCode { get; }
    public Dictionary<string, object?> ExtraData { get; set; } = new();
    public ApiException(LogLevel logLevel, HttpStatusCode statusCode, string message, Exception? innerException = null, Dictionary<string, object?>? extraData = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
        if (extraData is not null)
        {
            ExtraData = extraData;
        }
    }

    public IResult ToResult()
    {
        return Results.Problem(Message, null, (int)StatusCode);
    }
}