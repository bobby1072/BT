using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;

namespace BT.Common.Http.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        HttpRequestMessage request,
        IPollyRetrySettings retrySettings,
        CancellationToken cancellationToken = default
    )
    {
        var pipeline = retrySettings.ToPipeline();

        return await pipeline.ExecuteAsync(async ct =>
            await client.SendAsync(request, ct)
        ,cancellationToken);
    }
}
