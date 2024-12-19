using BT.Common.HttpClient.Models;

namespace BT.Common.HttpClient.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> SendAsync(this System.Net.Http.HttpClient client, HttpRequestMessage request,
        PollyRetrySettings retrySettings)
    {
        var pipeline = retrySettings.ToPipeline();
        
        return await pipeline.ExecuteAsync(async ct => await client.SendAsync(request, ct));
    }
}