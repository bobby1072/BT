using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace BT.Common.Http.Tests.TestHelpers;

public class StaticJsonHandler<T> : HttpMessageHandler where T: class
{
    private readonly T _json;
    private readonly HttpStatusCode _statusCode;
    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public StaticJsonHandler(T data,HttpStatusCode statusCode,JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _json = data;
        _statusCode = statusCode;
        _jsonSerializerOptions = jsonSerializerOptions;
    }
    protected sealed override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = _statusCode, Content = JsonContent.Create(_json, MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json),_jsonSerializerOptions)
        };

        return Task.FromResult(response);
    }
}