using BT.Common.Api.Helpers.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BT.Common.Api.Helpers.Extensions;

public static class DefaultEndpointExtensions
{
    public static WebApplication UseHealthGetEndpoints(this WebApplication application)
    {
        application.MapGet("/Api/Healthz/", (
                [FromServices]ILogger<ServiceInfo> logger,
                [FromServices]IOptions<ServiceInfo> serviceInfo
            ) =>
        {
            logger.LogInformation("Service appears healthy...");

            return new WebOutcome<HealthResponse>
            {
                Data = new HealthResponse
                {
                    ServiceInfo = serviceInfo.Value,
                },
            };
        })
        .WithName("Health");
        
        application.UseHealthChecks("/Api/Health");
        
        return application;
    }
}