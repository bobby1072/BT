using System.Net.Http.Json;
using System.Text.Json;
using BT.Common.Http.Models;
using HttpRequestException = BT.Common.Http.Exceptions.HttpRequestException;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static Task<T> GetJsonAsync<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Get;

        return requestBuilder.SendAndDeserializeJson<T>(httpClient, null, cancellationToken);
    }

    public static Task<T> GetJsonAsync<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        JsonSerializerOptions jsonSerializerOptions,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Get;

        return requestBuilder.SendAndDeserializeJson<T>(
            httpClient,
            jsonSerializerOptions,
            cancellationToken
        );
    }

    public static Task<string> GetStringAsync(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Get;

        return requestBuilder.SendAndReadString(httpClient, cancellationToken);
    }

    public static Task<T> PostJsonAsync<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Post;

        return requestBuilder.SendAndDeserializeJson<T>(httpClient, null, cancellationToken);
    }

    public static Task<T> PostJsonAsync<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        JsonSerializerOptions jsonSerializerOptions,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Post;

        return requestBuilder.SendAndDeserializeJson<T>(
            httpClient,
            jsonSerializerOptions,
            cancellationToken
        );
    }
    public static Task<string> PostStringAsync(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        requestBuilder.HttpMethod = HttpMethod.Post;

        return requestBuilder.SendAndReadString(httpClient, cancellationToken);
    }
    public static Task PostAsync(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        return requestBuilder.SendAsync(httpClient, HttpMethod.Post, cancellationToken);
    }
    public static async Task SendAsync(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        HttpMethod httpMethod,
        CancellationToken cancellationToken = default
    )
    {
        string? errorMessage = null;
        try
        {
            requestBuilder.HttpMethod = httpMethod;
            
            using var httpResponse = await httpClient.SendAsync(requestBuilder.ToHttpRequestMessage(), 
                cancellationToken);
            
            errorMessage = await CheckStatusAndGetErrorMessage(requestBuilder, httpResponse, cancellationToken);
            
            ThrowOnBadStatus(requestBuilder, httpResponse);
        }
        catch (System.Net.Http.HttpRequestException httpRequestException)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: httpRequestException.Message, httpRequestException.StatusCode, httpRequestException);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: ex.Message, null, ex);
        }
    }
    public static async Task GetAsync(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        string? errorMessage = null;
        try
        {
            using var httpResponse = await httpClient.GetAsync(requestBuilder.GetFinalUrl(), cancellationToken);
            
            errorMessage = await CheckStatusAndGetErrorMessage(requestBuilder, httpResponse, cancellationToken);
            
            ThrowOnBadStatus(requestBuilder, httpResponse);
        }
        catch (System.Net.Http.HttpRequestException httpRequestException)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: httpRequestException.Message, httpRequestException.StatusCode, httpRequestException);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: ex.Message, null, ex);
        }
    }
    private static async Task<T> SendAndDeserializeJson<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        string? errorMessage = null;
        try
        {
            using var httpResponse = requestBuilder.HttpMethod == HttpMethod.Get ?
                await httpClient.GetAsync(requestBuilder.GetFinalUrl(), cancellationToken) :
                await httpClient.PostAsync(requestBuilder.GetFinalUrl(), requestBuilder.Content, cancellationToken);

            errorMessage = await CheckStatusAndGetErrorMessage(requestBuilder, httpResponse, cancellationToken);
            
            ThrowOnBadStatus(requestBuilder, httpResponse);

            return await ReadFromJsonAsync<T>(httpResponse, jsonSerializerOptions, cancellationToken);
        }
        catch (System.Net.Http.HttpRequestException httpRequestException)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: httpRequestException.Message, httpRequestException.StatusCode, httpRequestException);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: ex.Message, null, ex);
        }
    }
    private static async Task<string> SendAndReadString(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        string? errorMessage = null;
        try
        {
            using var httpResponse = requestBuilder.HttpMethod == HttpMethod.Get ?
                await httpClient.GetAsync(requestBuilder.GetFinalUrl(), cancellationToken) :
                await httpClient.PostAsync(requestBuilder.GetFinalUrl(), requestBuilder.Content, cancellationToken);
            
            errorMessage = await CheckStatusAndGetErrorMessage(requestBuilder, httpResponse, cancellationToken);
            
            ThrowOnBadStatus(requestBuilder, httpResponse);
            
            var deserializedResponse =
                await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                ?? throw new HttpRequestException("Failed to read http response content");

            return deserializedResponse;
        }
        catch (System.Net.Http.HttpRequestException httpRequestException)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: httpRequestException.Message, httpRequestException.StatusCode, httpRequestException);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(string.IsNullOrWhiteSpace(errorMessage) ? errorMessage: ex.Message, null, ex);
        }
    }

    private static void ThrowOnBadStatus(HttpRequestBuilder requestBuilder, HttpResponseMessage httpResponse)
    {
                    
        if (!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length > 0 &&
            !requestBuilder.AllowedHttpStatusCodes.Contains(httpResponse.StatusCode))
        {
            httpResponse.EnsureSuccessStatusCode();
        }
        else if(!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length == 0)
        {
            httpResponse.EnsureSuccessStatusCode();
        }
    }
    private static async Task<string?> CheckStatusAndGetErrorMessage(HttpRequestBuilder requestBuilder,
        HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        string? errorMessage = null;

        if (!httpResponse.IsSuccessStatusCode)
        {
            errorMessage = requestBuilder.AsyncErrorExtractor is null ? 
                await httpResponse.TryReadStringFromResponse() :
                await requestBuilder.AsyncErrorExtractor.Invoke(httpResponse, cancellationToken);
        }
        
        return errorMessage;
    }
    private static async Task<T> ReadFromJsonAsync<T>(HttpResponseMessage responseMessage, JsonSerializerOptions? opts, CancellationToken cancellationToken)
    {
        var deserializedResponse = await responseMessage.Content.ReadFromJsonAsync<T>(
            opts,
            cancellationToken
        );

        if (deserializedResponse is null)
        {
            var jsonException = new JsonException(
                $"Failed to deserialize http response content to type of {typeof(T).Name}."
            );
            throw new HttpRequestException(jsonException.Message, null, jsonException);
        }

        return deserializedResponse;
    }
    private static async Task<string?> TryReadStringFromResponse(this HttpResponseMessage httpResponseMessage)
    {
        try
        {
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        catch
        {
            return null;
        }
    }
}
