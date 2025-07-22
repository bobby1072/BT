using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BT.Common.Helpers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSingletonOptions<TOptions>(this IServiceCollection services,
        IConfiguration configSection) where TOptions : class
    {
        services
            .Configure<TOptions>(configSection)
            .AddSingleton<TOptions>(sp => sp.GetRequiredService<IOptions<TOptions>>().Value);

        return services;
    }
    public static IServiceCollection ConfigureScopedOptions<TOptions>(this IServiceCollection services,
        IConfiguration configSection) where TOptions : class
    {
        services
            .Configure<TOptions>(configSection)
            .AddScoped<TOptions>(sp => sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Value);

        return services;
    }
}