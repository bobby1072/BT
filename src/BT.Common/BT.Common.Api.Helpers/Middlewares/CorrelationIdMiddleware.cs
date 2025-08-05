using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BT.Common.Api.Helpers.Middlewares;

internal sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
    {
        var correlationIdToUse = context.Request.Headers.TryGetValue(ApiConstants.CorrelationIdHeader, out var foundCorrelationId) 
            ? foundCorrelationId.ToString() : Guid.NewGuid().ToString();
        

        if (!context.Response.Headers.TryAdd(ApiConstants.CorrelationIdHeader, correlationIdToUse))
        {
            logger.LogWarning("Failed to add correlationId: {CorrelationId} to http response headers", correlationIdToUse);
        }
        else
        {
            logger.LogInformation("CorrelationId: {CorrelationId} added to request headers successfully", correlationIdToUse);
        }
        
        context.Items[ApiConstants.CorrelationIdHeader] = correlationIdToUse;

        using (logger.BeginScope(new Dictionary<string, object>
               {
                   [ApiConstants.CorrelationIdHeader] = correlationIdToUse
               }))
        {
            await _next.Invoke(context);
        }
    }
}