using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using BT.Common.Api.Helpers.Extensions;
using BT.Common.Api.Helpers.Models;
using BT.Common.Helpers;
using BT.Common.Helpers.Extensions;
using BT.Common.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BT.Common.Api.Helpers;

[ExcludeFromCodeCoverage]
public static class BTCommonWebApplicationBuilder
{
    public static WebApplication ConfigureWebApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseRouting();
        app.UseRequestTimeouts();
        app.UseResponseCompression();

        app.UseCorrelationIdMiddleware()
            .UseTraceParentResponseHeaderMiddleware()
            .UseBadRequestExceptionHandlingMiddleware();
        
        app.UseHealthGetEndpoints();
        
        return app;
    }
    
    
    public static WebApplicationBuilder CreateWebApplication(string[] args,
        (OpenApiInfo OpenApiInfo, string ApiName)? openApiInfo = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureWebApp(openApiInfo);
        
        return builder;
    }
    public static WebApplicationBuilder CreateWebApplication((OpenApiInfo OpenApiInfo, string ApiName)? openApiInfo = null)
    {
        var builder = WebApplication.CreateBuilder();

        builder.ConfigureWebApp(openApiInfo);
        
        return builder;
    }
    public static WebApplicationBuilder CreateWebApplication(string[] args,
        Action<SwaggerGenOptions> swaggerSetupAction)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureWebApp(null, swaggerSetupAction);
        
        return builder;
    }
    public static WebApplicationBuilder CreateWebApplication(Action<SwaggerGenOptions> swaggerSetupAction)
    {
        var builder = WebApplication.CreateBuilder();

        builder.ConfigureWebApp(null, swaggerSetupAction);
        
        return builder;
    }
    private static WebApplicationBuilder ConfigureWebApp(this WebApplicationBuilder builder, 
        (OpenApiInfo OpenApiInfo, string ApiName)? openApiInfo = null,
        Action<SwaggerGenOptions>? swaggerSetupAction = null)
    {
        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        var serviceInfo = builder.CheckAndAddSingletonOptions<ServiceInfo>();

        builder.Services.AddTelemetryServices(serviceInfo.ReleaseName);

        var requestTimeout = builder.Configuration.GetValue<int>("RequestTimeout");

        builder.Services.AddRequestTimeouts(opts =>
        {
            opts.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(requestTimeout > 0 ? requestTimeout : 60),
            };
        });
        
        builder.Services.AddHealthChecks();

        builder.Logging.AddJsonLogging();

        builder.Services.AddResponseCompression();
        builder.Services.AddProblemDetails();
        
        builder.Services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        builder.Services.AddEndpointsApiExplorer();

        if (openApiInfo != null)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    openApiInfo.Value.OpenApiInfo.Version,
                    openApiInfo.Value.OpenApiInfo
                );
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{openApiInfo.Value.ApiName}.xml"));
            });
        }
        else if (swaggerSetupAction != null)
        {
            builder.Services.AddSwaggerGen(swaggerSetupAction); 
        }

        return builder;
    }
}