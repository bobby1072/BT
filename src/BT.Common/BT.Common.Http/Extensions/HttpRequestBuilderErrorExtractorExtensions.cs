using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder AddErrorExtractor(this HttpRequestBuilder requestBuilder,
        Func<HttpResponseMessage, string?> errorExtractor)
    {
        requestBuilder.AddErrorExtractor(errorExtractor);
        
        return requestBuilder;
    }
}