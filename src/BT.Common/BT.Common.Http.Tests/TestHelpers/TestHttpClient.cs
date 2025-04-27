namespace AiTrainer.Web.TestBase;

public class TestHttpClient: HttpClient
{
    private readonly string _expectedUrl;
    private string? _actualUri;
    private Dictionary<string, string> _headersCalled = [];
    
    public TestHttpClient(HttpMessageHandler handler, string expectedUrl) : base(handler)
    {
        _expectedUrl = expectedUrl;
    }

    public void ShouldHaveCalledExpectedUrl()
    {
        Assert.Equal(_expectedUrl, _actualUri);
    }

    public void ShouldHaveUsedHeader(string headerName, string headerValue)
    {
        Assert.Contains(_headersCalled, h => h.Key == headerName && h.Value == headerValue);
    }

    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _actualUri = request.RequestUri?.ToString();
        foreach (var header in request.Headers)
        {
            _headersCalled.Add(header.Key, header.Value.FirstOrDefault() ?? string.Empty);
        }
        return base.SendAsync(request, cancellationToken);
    }
}