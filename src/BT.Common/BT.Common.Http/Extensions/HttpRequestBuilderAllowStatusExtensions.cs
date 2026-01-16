using System.Net;
using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder AllowHttpStatusCodes(this HttpRequestBuilder builder,
        params HttpStatusCode[] allowedStatusCodes)
    {
        builder.AddHttpStatusCodes(allowedStatusCodes);
        
        return builder;
    }

    public static HttpRequestBuilder AllowHttpStatusCodes(this string uri,
        params HttpStatusCode[] allowedStatusCodes)
    {
        var builder = new HttpRequestBuilder(new Uri(uri));
        
        builder.AddHttpStatusCodes(allowedStatusCodes);
        
        return builder;
    }

    public static HttpRequestBuilder AllowHttpStatusCodes(this Uri uri,
        params HttpStatusCode[] allowedStatusCodes)
    {
        var builder = new HttpRequestBuilder(uri);
        
        builder.AddHttpStatusCodes(allowedStatusCodes);
        
        return builder;
    }
}