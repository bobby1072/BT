using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;

namespace BT.Common.HttpClient.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> SendAsync(
        this System.Net.Http.HttpClient client,
        HttpRequestMessage request,
        IPollyRetrySettings retrySettings,
        CancellationToken cancellationToken = default
    )
    {
        var pipeline = retrySettings.ToPipeline();

        return await pipeline.ExecuteAsync(async ct =>
            await client.SendAsync(request, cancellationToken)
        );
    }
}
