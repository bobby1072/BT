using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BT.Common.Http.Exceptions;
using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static Task<T> GetJsonAsync<T>(this HttpRequestBuilder requestBuilder, HttpClient httpClient, CancellationToken cancellationToken = default) where T : notnull
    {
        requestBuilder.HttpMethod = HttpMethod.Get;
        
        return requestBuilder.SendAndDeserializeJson<T>(httpClient, null,  cancellationToken);
    }
    public static Task<T> GetJsonAsync<T>(this HttpRequestBuilder requestBuilder, HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default) where T : notnull
    {
        requestBuilder.HttpMethod = HttpMethod.Get;
        
        return requestBuilder.SendAndDeserializeJson<T>(httpClient, jsonSerializerOptions,  cancellationToken);
    }
    public static Task<T> PostJsonAsync<T>(this HttpRequestBuilder requestBuilder, HttpClient httpClient, CancellationToken cancellationToken = default) where T : notnull
    {
        requestBuilder.HttpMethod = HttpMethod.Post;
        
        return requestBuilder.SendAndDeserializeJson<T>(httpClient, null,  cancellationToken);
    }
    public static Task<T> PostJsonAsync<T>(this HttpRequestBuilder requestBuilder, HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default) where T : notnull
    {
        requestBuilder.HttpMethod = HttpMethod.Post;
        
        return requestBuilder.SendAndDeserializeJson<T>(httpClient, jsonSerializerOptions,  cancellationToken);
    }
    
    
    
    private static async Task<T> SendAndDeserializeJson<T>(this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken cancellationToken = default) where T : notnull
    {
        var httpReqMessage = requestBuilder.ToHttpRequestMessage();
        
        using var httpResponse = await httpClient.SendAsync(httpReqMessage, cancellationToken);
        
        httpResponse.EnsureSuccessStatusCode();
        
        var deserializedResponse = 
            await httpResponse.Content.ReadFromJsonAsync<T>(cancellationToken)
            ?? throw new JsonException($"Failed to deserialize http response content to type of {typeof(T).Name}.");
        
        return deserializedResponse;  
    } 
    private static HttpRequestMessage ToHttpRequestMessage(this HttpRequestBuilder requestBuilder)
    {
        if (!requestBuilder.IsValidRequest())
        {
            var sb = new StringBuilder();

            foreach (var error in requestBuilder.GetRequestValidationErrors())
            {
                sb.AppendLine($"{error} ");
            }
            throw new HttpRequestBuilderException(sb.ToString().Trim());
        }
        
        var httpRequestMessage = new HttpRequestMessage(requestBuilder.HttpMethod!, requestBuilder.RequestUri);

        if (requestBuilder.Content is not null)
        {
            httpRequestMessage.Content = requestBuilder.Content;
        }

        foreach (var header in requestBuilder.Headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }
        
        return httpRequestMessage;
    }
}