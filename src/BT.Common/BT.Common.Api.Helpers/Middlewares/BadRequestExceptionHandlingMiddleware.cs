using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Middlewares;

internal sealed class BadRequestExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public BadRequestExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<BadRequestExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BadHttpRequestException ex) when (ex.InnerException is JsonException jsonEx)
        {
            logger.LogError(ex, "Bad Request exception occurred during request with message: {ExMessage}",
                ex.Message);

            logger.LogError(ex.InnerException, "Inner json exception occurred during request with message: {ExMessage}",
                ex.InnerException.Message);

            await ProduceBadRequestResponse(context, jsonEx.Message);
        }
        catch (BadHttpRequestException ex) when (ex.StatusCode is 400)
        {
            logger.LogError(ex, "Bad Request exception occurred during request with message: {ExMessage}",
                ex.Message);
            
            await ProduceBadRequestResponse(context);
        }
    }

    private static async Task ProduceBadRequestResponse(HttpContext context, string? detail = null)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;

        var problem = new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Missing required header: X-Custom-Header",
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
        return;
    }
}