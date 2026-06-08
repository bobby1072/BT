using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using BT.Common.Http.Extensions;
using BT.Common.Http.Models;

namespace BT.Common.Http.Tests.ExtensionsTests;

public class HttpRequestBuilderContentExtensionsTests
{
    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Fact]
    public void WithApplicationJson_SetsJsonContent()
    {
        // Arrange
        var requestBuilder = new HttpRequestBuilder(new Uri("https://localhost:5000"));
        var testObject = new TestObject { Name = "John", Age = 30 };
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        // Act
        requestBuilder.WithApplicationJson(testObject, options);

        // Assert
        Assert.NotNull(requestBuilder.Content);
        Assert.IsType<JsonContent>(requestBuilder.Content);

        var content = requestBuilder.Content as JsonContent;
        Assert.Equal(MediaTypeNames.Application.Json, content!.Headers.ContentType!.MediaType);
    }

    [Fact]
    public void WithMultipartFormData_SetsMultipartContent_WithSyncAction()
    {
        // Arrange
        var requestBuilder = new HttpRequestBuilder(new Uri("https://localhost:5000"));

        // Act
        requestBuilder.WithMultipartFormData(form =>
        {
            form.Add(new StringContent("test value"), "field1");
        });

        // Assert
        Assert.NotNull(requestBuilder.Content);
        Assert.IsType<MultipartFormDataContent>(requestBuilder.Content);

        var multipart = requestBuilder.Content as MultipartFormDataContent;
        Assert.Single(multipart!);
    }

    [Fact]
    public async Task WithMultipartFormData_SetsMultipartContent_WithAsyncAction()
    {
        // Arrange
        var requestBuilder = new HttpRequestBuilder(new Uri("https://localhost:5000"));

        // Act
        await requestBuilder.WithMultipartFormData(async form =>
        {
            await Task.Delay(10); // Simulate async operation
            form.Add(new StringContent("test value async"), "fieldAsync");
        });

        // Assert
        Assert.NotNull(requestBuilder.Content);
        Assert.IsType<MultipartFormDataContent>(requestBuilder.Content);
        
        var multipart = requestBuilder.Content as MultipartFormDataContent;
        Assert.Single(multipart!);
    }
}