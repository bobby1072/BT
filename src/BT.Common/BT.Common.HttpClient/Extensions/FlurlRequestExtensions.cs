using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;
using Flurl.Http;

namespace BT.Common.HttpClient.Extensions;

public static class FlurlRequestExtensions
{
    public static Task<TReturn> GetJsonAsync<TReturn>(this FlurlRequest request,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        return RetryRequest(() => request.GetJsonAsync<TReturn>(), pollyRetrySettings, cancellationToken);
    }
    public static Task<IFlurlResponse> PostJsonAsync<TBody>(this FlurlRequest request,
        TBody requestBody,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        return RetryRequest<IFlurlResponse>(() => request.PostJsonAsync(requestBody, cancellationToken: cancellationToken), pollyRetrySettings, cancellationToken);
    }
    public static Task<IFlurlResponse> GetAsync<TReturn>(this FlurlRequest request,
        IPollyRetrySettings pollyRetrySettings,
        CancellationToken cancellationToken = default)
    {
        return RetryRequest(() => request.GetAsync(), pollyRetrySettings, cancellationToken);
    }
    private static async Task<TReturn> RetryRequest<TReturn>(this Func<Task<TReturn>> flurlRequest, IPollyRetrySettings retrySettings, CancellationToken cancellationToken = default)
    {
        var pipeline = retrySettings.ToPipeline();
        
        return await pipeline.ExecuteAsync(async (ct) => await flurlRequest.Invoke(), cancellationToken);
    }
}