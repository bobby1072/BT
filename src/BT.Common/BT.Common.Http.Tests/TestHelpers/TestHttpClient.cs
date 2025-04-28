namespace AiTrainer.Web.TestBase;

public class TestHttpClient: HttpClient
{
    private string? _actualUri;
    private readonly Dictionary<string, string> _headersCalled = [];
    private HttpMethod? _methodUsed;
    public TestHttpClient(HttpMessageHandler handler) : base(handler) { }
    public void ShouldHaveCalledExpectedUrl(string expectedUrl)
    {
        Assert.Equal(expectedUrl, _actualUri);
    }

    public void ShouldHaveUsedHeader(string headerName, string headerValue)
    {
        Assert.Contains(_headersCalled, h => h.Key == headerName && h.Value == headerValue);
    }

    public void ShouldHaveUsedMethod(HttpMethod method)
    {
        Assert.Equal(_methodUsed, method);
    }
    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _actualUri = request.RequestUri?.ToString();
        _methodUsed = request.Method;
        foreach (var header in request.Headers)
        {
            _headersCalled.Add(header.Key, header.Value.FirstOrDefault() ?? string.Empty);
        }
        return base.SendAsync(request, cancellationToken);
    }
}