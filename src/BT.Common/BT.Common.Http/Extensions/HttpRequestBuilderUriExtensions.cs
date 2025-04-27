using System.Net;
using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder WithAuthorizationHeader(this string baseUri, string value)
    {
        var reqBuilder = new HttpRequestBuilder(new Uri(baseUri));
        reqBuilder.AddHeader(HttpRequestHeader.Authorization.ToString(), value);
        
        return reqBuilder;
    }
    public static HttpRequestBuilder WithAuthorizationHeader(this Uri baseUri, string value)
    {
        var reqBuilder = new HttpRequestBuilder(baseUri);
        reqBuilder.AddHeader(HttpRequestHeader.Authorization.ToString(), value);
        
        return reqBuilder;
    }
    
    public static HttpRequestBuilder WithAuthorizationHeader(this HttpRequestBuilder reqBuilder, string value)
    {
        reqBuilder.AddHeader(HttpRequestHeader.Authorization.ToString(), value);
        
        return reqBuilder;
    }
    
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
        return new HttpRequestBuilder(new Uri((baseUrl.TrimPath() + "/" + path.TrimPath()).TrimPath())); 
    }
    
    public static HttpRequestBuilder AppendPathSegment(this Uri baseUrl, string path)
    {
        return new HttpRequestBuilder(new Uri((baseUrl.AbsoluteUri.TrimPath() + "/" + path.TrimPath()).TrimPath()));
    }

    public static HttpRequestBuilder AppendPathSegment(this HttpRequestBuilder requestBuilder, string path)
    {
        requestBuilder.RequestUri = new Uri((requestBuilder.RequestUri.AbsoluteUri.TrimPath() + "/" + path.TrimPath()).TrimPath());

        return requestBuilder;
    }

    public static Uri ToUri(this string requestBuilder)
    {
        return new Uri(requestBuilder);
    }
    public static HttpRequestBuilder ToHttpRequestBuilder(this Uri requestUri)
    {
        return new HttpRequestBuilder(requestUri);
    }
    private static string TrimPath(this string pathSegment)
    {
        return pathSegment.Trim().Trim('/').Trim('\\');
    }
}