using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder WithHeader(this string baseUri, string key, string value)
    {
        var reqBuilder = new HttpRequestBuilder(new Uri(baseUri));
        reqBuilder.AddHeader(key, value);
        
        return reqBuilder;
    }
    public static HttpRequestBuilder WithHeader(this Uri baseUri, string key, string value)
    {
        var reqBuilder = new HttpRequestBuilder(baseUri);
        reqBuilder.AddHeader(key, value);
        
        return reqBuilder;
    }
    
    public static HttpRequestBuilder WithHeader(this HttpRequestBuilder reqBuilder, string key, string value)
    {
        reqBuilder.AddHeader(key, value);
        
        return reqBuilder;
    }
    public static HttpRequestBuilder AppendPathSegment(this string baseUrl, string path)
    {
        return new HttpRequestBuilder(new Uri(baseUrl.TrimPath() + "/" + path.TrimPath() + "/")); 
    }
    
    public static HttpRequestBuilder AppendPathSegment(this Uri baseUrl, string path)
    {
        return new HttpRequestBuilder(new Uri(baseUrl.AbsoluteUri.TrimPath() + "/" + path.TrimPath() + "/"));
    }

    public static HttpRequestBuilder AppendPathSegment(this HttpRequestBuilder requestBuilder, string path)
    {
        requestBuilder.RequestUri = new Uri(requestBuilder.RequestUri.AbsoluteUri.TrimPath() + "/" + path.TrimPath() + "/");

        return requestBuilder;
    }
    private static string TrimPath(this string pathSegment)
    {
        return pathSegment.Trim().Trim('/');
    }
}