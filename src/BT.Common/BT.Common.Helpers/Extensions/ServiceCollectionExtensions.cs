using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BT.Common.Helpers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSingletonOptions<TOptions>(this IServiceCollection services,
        TOptions opts) where TOptions : class
    {
        services
            .AddSingleton(Options.Create(opts))
            .AddSingleton<TOptions>(sp => sp.GetRequiredService<IOptions<TOptions>>().Value);

        return services;
    }
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
    
    
    public static T CheckAndAddSingletonOptions<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? nameofSection = null
    )
        where T : class
    {
        var sectname = nameofSection ?? typeof(T).Name;

        var configSection = configuration.GetSection(sectname);

        if (!configSection.Exists())
        {
            throw new ArgumentException(sectname);
        }

        services.ConfigureSingletonOptions<T>(configSection);

        return configSection.Get<T>() ?? throw new ArgumentException(sectname);
    }   
}