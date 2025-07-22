using BT.Common.Api.Helpers.Midddlewares;
using Microsoft.AspNetCore.Builder;

namespace BT.Common.Api.Helpers.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseCorrelationIdMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        
        return app;
    }
}