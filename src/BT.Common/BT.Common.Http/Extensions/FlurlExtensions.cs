using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace BT.Common.Http.Extensions;

public static class FlurlExtensions
{
    public static IFlurlRequest WithSerializer(this Uri uri, ISerializer serializer)
    {
        return uri.WithSettings(x=> { x.JsonSerializer = serializer; });
    }
    public static IFlurlRequest WithSerializer(this IFlurlRequest uri, ISerializer serializer)
    {
        return uri.WithSettings(x=> { x.JsonSerializer = serializer; });
    }
    
    public static Task<TReturn> GetJsonAsync<TReturn>(this Url request,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        return RetryRequest(() => request.GetJsonAsync<TReturn>(cancellationToken: cancellationToken), pollyRetrySettings, cancellationToken);
    }
    public static Task<TReturn> GetJsonAsync<TReturn>(this IFlurlRequest request,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        var originalRequest = pollyRetrySettings.TimeoutInSeconds is null ? request: request
            .WithTimeout(TimeSpan.FromSeconds((double)pollyRetrySettings.TimeoutInSeconds!));

        return RetryRequest(() => originalRequest.GetJsonAsync<TReturn>(cancellationToken: cancellationToken), pollyRetrySettings, cancellationToken);
    }
    public static Task<IFlurlResponse> GetAsync(this IFlurlRequest request,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        var originalRequest = pollyRetrySettings.TimeoutInSeconds is null ? request : request
            .WithTimeout(TimeSpan.FromSeconds((double)pollyRetrySettings.TimeoutInSeconds!));

        return RetryRequest(() => originalRequest.GetAsync(cancellationToken: cancellationToken), pollyRetrySettings, cancellationToken);
    }
    public static Task<TReturn> ReceiveJsonAsync<TReturn>(this Task<IFlurlResponse> response, IPollyRetrySettings pollyRetrySettings, CancellationToken cancellationToken = default)
    {
        return RetryRequest(response.ReceiveJson<TReturn>, pollyRetrySettings, cancellationToken);
    }
    private static async Task<TReturn> RetryRequest<TReturn>(this Func<Task<TReturn>> flurlRequest, IPollyRetrySettings retrySettings, CancellationToken cancellationToken = default)
    {
        var pipeline = retrySettings.ToPipeline();
        
        return await pipeline.ExecuteAsync(async (ct) => await flurlRequest.Invoke(), cancellationToken);
    }
}