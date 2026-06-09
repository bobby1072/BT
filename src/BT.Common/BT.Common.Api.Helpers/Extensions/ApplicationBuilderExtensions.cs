using System.Diagnostics;
using BT.Common.Api.Helpers.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BT.Common.Api.Helpers.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        
        return app;
    }
    
    public static IApplicationBuilder UseBadRequestExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<BadRequestExceptionHandlingMiddleware>();
        
        return app;
    }
    
    public static IApplicationBuilder UseTraceParentResponseHeader(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.OnStarting(() =>
            {
                var activity = Activity.Current;
                if (activity is not null)
                {
                    context.Response.Headers.TraceParent = activity.Id;
                }
                return Task.CompletedTask;
            });

            await next.Invoke();
        });

        return app;
    }
}