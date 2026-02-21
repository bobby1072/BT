using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BT.Common.Helpers.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static T CheckAndAddSingletonOptions<T>(
        this IHostApplicationBuilder hostAppBuilder,
        string? nameofSection = null
    )
        where T : class
    {
        var sectname = nameofSection ?? typeof(T).Name;

        var configSection = hostAppBuilder.Configuration.GetSection(sectname);

        if (!configSection.Exists())
        {
            throw new ArgumentException(sectname);
        }

        hostAppBuilder.Services.ConfigureSingletonOptions<T>(configSection);

        return configSection.Get<T>() ?? throw new ArgumentException(sectname);
    }   
}