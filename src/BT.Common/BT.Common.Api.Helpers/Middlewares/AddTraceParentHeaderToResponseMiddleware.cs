using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace BT.Common.Api.Helpers.Middlewares;

public sealed class AddTraceParentHeaderToResponseMiddleware
{
    private readonly RequestDelegate _next;

    public AddTraceParentHeaderToResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next.Invoke(context);
        var activ = Activity.Current;
        if (!string.IsNullOrWhiteSpace(activ?.Id))
        {
            context.Response.Headers.TraceParent = activ?.Id;
        }
    }
}