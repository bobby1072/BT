using System.Net;
using BT.Common.Http.Extensions;
using BT.Common.Http.Models;
using BT.Common.Http.Tests.TestHelpers;

namespace BT.Common.Http.Tests.ExtensionsTests;

public class HttpRequestBuilderExtensionsTests
{
    private class TestDto
    {
        public string Name { get; set; } = string.Empty;
    }

    private static HttpRequestBuilder CreateValidRequestBuilder(string url = "https://example.com")
    {
        return new HttpRequestBuilder(new Uri(url));
    }

    [Fact]
    public async Task GetJsonAsync_ShouldDeserializeSuccessfully()
    {
        // Arrange
        var expected = new TestDto { Name = "John Doe" };
        var handler = new StaticJsonHandler<TestDto>(expected, HttpStatusCode.OK);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();

        // Act
        var result = await requestBuilder.GetJsonAsync<TestDto>(client);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Get);
    }

    [Fact]
    public async Task PostJsonAsync_ShouldDeserializeSuccessfully()
    {
        // Arrange
        var expected = new TestDto { Name = "Jane Doe" };
        var handler = new StaticJsonHandler<TestDto>(expected, HttpStatusCode.OK);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();
        requestBuilder.HttpMethod = HttpMethod.Post;

        // Act
        var result = await requestBuilder.PostJsonAsync<TestDto>(client);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Post);
    }

    [Fact]
    public async Task GetJsonAsync_ShouldThrow_OnFailedHttpStatus()
    {
        // Arrange
        var expected = new TestDto { Name = "Broken" };
        var handler = new StaticJsonHandler<TestDto>(expected, HttpStatusCode.BadRequest);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => requestBuilder.GetJsonAsync<TestDto>(client));
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Get);
    }

    [Fact]
    public async Task GetStringAsync_ShouldReadContentSuccessfully()
    {
        // Arrange
        var expectedContent = "Plain Text Response";
        var handler = new StaticJsonHandler<string>(expectedContent, HttpStatusCode.OK);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();

        // Act
        var result = await requestBuilder.GetStringAsync(client);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result));
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Get);
    }
    [Fact]
    public async Task PostJsonAsync_ShouldThrow_OnFailedHttpStatus()
    {
        // Arrange
        var expected = new TestDto { Name = "Broken Post" };
        var handler = new StaticJsonHandler<TestDto>(expected, HttpStatusCode.BadRequest);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();
        requestBuilder.HttpMethod = HttpMethod.Post;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => requestBuilder.PostJsonAsync<TestDto>(client));
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Post);
    }

    [Fact]
    public async Task PostJsonAsync_ShouldThrow_OnDeserializationFailure()
    {
        // Arrange
        var handler = new StaticJsonHandler<string>("invalid json for TestDto", HttpStatusCode.OK);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();
        requestBuilder.HttpMethod = HttpMethod.Post;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => requestBuilder.PostJsonAsync<TestDto>(client));
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Post);
    }
    
    [Fact]
    public async Task PostStringAsync_ShouldThrow_OnFailedHttpStatus()
    {
        // Arrange
        var handler = new StaticJsonHandler<string>("Error Content", HttpStatusCode.InternalServerError);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();
        requestBuilder.HttpMethod = HttpMethod.Post;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => requestBuilder.PostStringAsync(client));
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
        client.ShouldHaveUsedMethod(HttpMethod.Post);
    }
}