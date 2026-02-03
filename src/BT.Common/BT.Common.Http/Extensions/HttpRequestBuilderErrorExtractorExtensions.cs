using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder AddAsyncErrorExtractor(this HttpRequestBuilder requestBuilder,
        Func<HttpResponseMessage, CancellationToken, Task<string?>> errorExtractor)
    {
        requestBuilder.AddAsyncErrorExtractor(errorExtractor);
        
        return requestBuilder;
    }
    public static HttpRequestBuilder AddErrorExtractor(this HttpRequestBuilder requestBuilder,
        Func<HttpResponseMessage, string?> errorExtractor)
    {
        requestBuilder.AddAsyncErrorExtractor((x,ct) => Task.FromResult(errorExtractor.Invoke(x)));
        
        return requestBuilder;
    }
}