using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BT.Common.Http.Exceptions;
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
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                errorMessage = requestBuilder.ErrorExtractor is null ? 
                    await httpResponse.TryReadStringFromResponse() :
                    await requestBuilder.ErrorExtractor.Invoke(httpResponse);
            }
            
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
            using var httpResponse = await httpClient.GetAsync(requestBuilder.RequestUri, cancellationToken);
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                errorMessage = requestBuilder.ErrorExtractor is null ? 
                    await httpResponse.TryReadStringFromResponse() :
                    await requestBuilder.ErrorExtractor.Invoke(httpResponse);
            }
            
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
            using var httpReqMessage = requestBuilder.ToHttpRequestMessage();

            using var httpResponse = await httpClient.SendAsync(httpReqMessage, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                errorMessage = requestBuilder.ErrorExtractor is null
                    ? await httpResponse.TryReadStringFromResponse()
                    : await requestBuilder.ErrorExtractor.Invoke(httpResponse);
            }

            if (!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length > 0 &&
                !requestBuilder.AllowedHttpStatusCodes.Contains(httpResponse.StatusCode))
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            else if (!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length == 0)
            {
                httpResponse.EnsureSuccessStatusCode();
            }


            var deserializedResponse = await httpResponse.Content.ReadFromJsonAsync<T>(
                jsonSerializerOptions,
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
            using var httpReqMessage = requestBuilder.ToHttpRequestMessage();
            using var httpResponse = await httpClient.SendAsync(httpReqMessage, cancellationToken);
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                errorMessage = requestBuilder.ErrorExtractor is null ? 
                    await httpResponse.TryReadStringFromResponse() :
                    await requestBuilder.ErrorExtractor.Invoke(httpResponse);
            }
            
            if (!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length > 0 &&
                !requestBuilder.AllowedHttpStatusCodes.Contains(httpResponse.StatusCode))
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            else if(!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length == 0)
            {
                httpResponse.EnsureSuccessStatusCode();
            }

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
