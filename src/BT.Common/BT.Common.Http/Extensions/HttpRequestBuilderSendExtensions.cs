using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BT.Common.Http.Exceptions;
using BT.Common.Http.Models;

namespace BT.Common.Http.Extensions;

public static partial class HttpRequestBuilderExtensions
{
    public static Task GetAsync(this HttpRequestBuilder builder, 
        HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        builder.HttpMethod = HttpMethod.Get;

        return httpClient.GetAsync(builder.RequestUri, cancellationToken);
    }
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

    public static Task PostAsync(this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        requestBuilder.HttpMethod = HttpMethod.Post;

        return httpClient.PostAsync(requestBuilder.RequestUri,
            requestBuilder.Content,
            cancellationToken);
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

    private static async Task<T> SendAndDeserializeJson<T>(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        JsonSerializerOptions? jsonSerializerOptions = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            using var httpReqMessage = requestBuilder.ToHttpRequestMessage();

            using var httpResponse = await httpClient.SendAsync(httpReqMessage, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length > 0 &&
                !requestBuilder.AllowedHttpStatusCodes.Contains(httpResponse.StatusCode))
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            else if(!httpResponse.IsSuccessStatusCode && requestBuilder.AllowedHttpStatusCodes.Length == 0)
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
                throw new HttpRequestException(jsonException.Message, jsonException);
            }

            return deserializedResponse;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(ex.Message, ex);
        }
    }

    private static async Task<string> SendAndReadString(
        this HttpRequestBuilder requestBuilder,
        HttpClient httpClient,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            using var httpReqMessage = requestBuilder.ToHttpRequestMessage();
            using var httpResponse = await httpClient.SendAsync(httpReqMessage, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var deserializedResponse =
                await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                ?? throw new HttpRequestException("Failed to read http response content");

            return deserializedResponse;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(ex.Message, ex);
        }
    }


}
