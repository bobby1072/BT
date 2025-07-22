using BT.Common.Api.Helpers.Midddlewares;
using Microsoft.AspNetCore.Builder;

namespace BT.Common.Api.Helpers.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        
        return app;
    }
}