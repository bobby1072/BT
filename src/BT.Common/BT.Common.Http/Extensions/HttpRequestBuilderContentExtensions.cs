using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static HttpRequestBuilder WithApplicationJson<T>(this HttpRequestBuilder reqBuilder, T jsonObject, JsonSerializerOptions? jsonSerializerOptions = null) where T : notnull
    {
        reqBuilder.Content = JsonContent.Create(jsonObject, MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json), jsonSerializerOptions);
        
        return reqBuilder;
    }

    public static HttpRequestBuilder WithMultipartFormData(this HttpRequestBuilder requestBuilder,
        Action<MultipartFormDataContent> formContentAction)
    {
        var formContent = new MultipartFormDataContent();
        formContentAction.Invoke(formContent);
        
        requestBuilder.Content = formContent;
        
        return requestBuilder;
    }
    public static async Task<HttpRequestBuilder> WithMultipartFormData(this HttpRequestBuilder requestBuilder,
        Func<MultipartFormDataContent, Task> formContentAction)
    {
        var formContent = new MultipartFormDataContent();
        await formContentAction.Invoke(formContent);
        
        requestBuilder.Content = formContent;
        
        return requestBuilder;
    }
}