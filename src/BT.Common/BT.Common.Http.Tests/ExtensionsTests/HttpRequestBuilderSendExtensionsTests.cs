using System.Net;
using AiTrainer.Web.TestBase;
using AiTrainer.Web.TestBase.Helpers;
using BT.Common.Http.Extensions;
using BT.Common.Http.Models;

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
    }

    [Fact]
    public async Task ShouldCaptureHeadersCorrectly()
    {
        // Arrange
        var expected = new TestDto { Name = "Header Test" };
        var handler = new StaticJsonHandler<TestDto>(expected, HttpStatusCode.OK);
        var client = new TestHttpClient(handler);

        var requestBuilder = CreateValidRequestBuilder();
        requestBuilder.Headers.Add("X-Test-Header", "TestValue");

        // Act
        var result = await requestBuilder.GetJsonAsync<TestDto>(client);

        // Assert
        Assert.NotNull(result);
        client.ShouldHaveUsedHeader("X-Test-Header", "TestValue");
        client.ShouldHaveCalledExpectedUrl("https://example.com/");
    }
}