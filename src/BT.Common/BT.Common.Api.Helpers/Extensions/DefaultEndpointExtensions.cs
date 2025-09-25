using BT.Common.Api.Helpers.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BT.Common.Api.Helpers.Extensions;

public static class DefaultEndpointExtensions
{
    public static WebApplication UseHealthGetEndpoint(this WebApplication application)
    {
        application.MapGet("/Api/Health/", (
                [FromServices]ILogger logger,
                [FromServices]IOptions<ServiceInfo> serviceInfo
            ) =>
        {
            logger.LogInformation("Service appears healthy...");

            return Task.FromResult((ActionResult<WebOutcome<HealthResponse>>)new WebOutcome<HealthResponse>
            {
                Data = new HealthResponse
                {
                    ServiceInfo = serviceInfo.Value,
                },
            });
        });
        
        
        return application;
    }
}