using System.Net;
using BT.Common.Http.Extensions;
using BT.Common.Http.Models;

namespace BT.Common.Http.Tests.ExtensionsTests;

public sealed class HttpRequestBuilderUriExtensionsTests
{
    
    [Theory]
    [InlineData("http://localhost:5000", "key", "value", "http://localhost:5000/?key=value")]
    [InlineData("http://localhost:5000/api", "search", "test", "http://localhost:5000/api?search=test")]
    [InlineData("http://localhost:5000/path?existing=1", "new", "2", "http://localhost:5000/path?existing=1&new=2")]
    public void String_AppendQueryParameter_Should_Add_Query_Correctly(string baseUrl, string key, string value, string expected)
    {
        // Act
        var builder = baseUrl.AppendQueryParameter(key, value);

        // Assert
        Assert.Equal(expected, builder.GetFinalUrl().AbsoluteUri);
    }

    [Theory]
    [InlineData("http://localhost:5000", "key", "value", "http://localhost:5000/?key=value")]
    [InlineData("http://localhost:5000/api", "search", "test", "http://localhost:5000/api?search=test")]
    [InlineData("http://localhost:5000/path?existing=1", "new", "2", "http://localhost:5000/path?existing=1&new=2")]
    public void Uri_AppendQueryParameter_Should_Add_Query_Correctly(string baseUrl, string key, string value, string expected)
    {
        // Act
        var builder = new Uri(baseUrl).AppendQueryParameter(key, value);

        // Assert
        Assert.Equal(expected, builder.GetFinalUrl().AbsoluteUri);
    }

    [Theory]
    [InlineData("http://localhost:5000", "key", "value", "http://localhost:5000/?key=value")]
    [InlineData("http://localhost:5000/api", "search", "test", "http://localhost:5000/api?search=test")]
    [InlineData("http://localhost:5000/path?existing=1", "new", "2", "http://localhost:5000/path?existing=1&new=2")]
    public void Builder_AppendQueryParameter_Should_Add_Query_Correctly(string baseUrl, string key, string value, string expected)
    {
        // Arrange
        var builder = new Uri(baseUrl).ToHttpRequestBuilder();

        // Act
        var updated = builder.AppendQueryParameter(key, value);

        // Assert
        Assert.Equal(expected, updated.GetFinalUrl().AbsoluteUri);
    }
    [Fact]
    public void String_WithAuthorizationHeader_Should_Add_Authorization_Header_Correctly()
    {
        //Arrange
        var testBaseUrl = "http://localhost:5000";
        
        //Act
        var requestBuilder = testBaseUrl.WithAuthorizationHeader("test");
        
        //Assert
        Assert.Contains(requestBuilder.Headers, x => x.Key == HttpRequestHeader.Authorization.ToString() && x.Value == "test");
    }
    [Fact]
    public void RequestBuilder_WithAuthorizationHeader_Should_Add_Authorization_Header_Correctly()
    {
        //Arrange
        var testBaseUrl = "http://localhost:5000".ToUri().ToHttpRequestBuilder();
        
        //Act
        var requestBuilder = testBaseUrl.WithAuthorizationHeader("test");
        
        //Assert
        Assert.Contains(requestBuilder.Headers, x => x.Key == HttpRequestHeader.Authorization.ToString() && x.Value == "test");
    }
    [Fact]
    public void Uri_WithAuthorizationHeader_Should_Add_Authorization_Header_Correctly()
    {
        //Arrange
        var testBaseUrl = "http://localhost:5000".ToUri();
        
        //Act
        var requestBuilder = testBaseUrl.WithAuthorizationHeader("test");
        
        //Assert
        Assert.Contains(requestBuilder.Headers, x => x.Key == HttpRequestHeader.Authorization.ToString() && x.Value == "test");
    }
    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data))]
    public void String_AppendPathSegment_Should_Build_Uris_Correctly(IReadOnlyCollection<string> splitPath,
        string expectedPath)
    {
        var startingBaseUrlString = splitPath.First();
        HttpRequestBuilder? requestBuilder = null;
        for (int i = 0; i < splitPath.Count; i++)
        {
            var segment = splitPath.ElementAt(i);            
            if (i == 0)
            {
                continue;
            }
            else if (i == 1)
            {
                requestBuilder = startingBaseUrlString?.AppendPathSegment(segment);
            }
            else
            {
                requestBuilder = requestBuilder?.AppendPathSegment(segment);
            }
        }

        if (requestBuilder == null)
        {
            requestBuilder = startingBaseUrlString?.ToUri().ToHttpRequestBuilder();
        }
        Assert.True(requestBuilder?.RequestUri.AbsoluteUri == expectedPath || requestBuilder?.RequestUri.AbsoluteUri == (expectedPath + "/"));
    }
    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data))]
    public void Uri_AppendPathSegment_Should_Build_Uris_Correctly(IReadOnlyCollection<string> splitPath,
        string expectedPath)
    {
        var startingBaseUrlUri = new Uri(splitPath.First());
        HttpRequestBuilder? requestBuilder = null;
        for (int i = 0; i < splitPath.Count; i++)
        {
            var segment = splitPath.ElementAt(i);            
            if (i == 0)
            {
                continue;
            }else if (i == 1)
            {
                requestBuilder = startingBaseUrlUri?.AppendPathSegment(segment);
            }
            else
            {
                requestBuilder = requestBuilder?.AppendPathSegment(segment);
            }
        }
        if (requestBuilder == null)
        {
            requestBuilder = startingBaseUrlUri?.ToHttpRequestBuilder();
        }
        Assert.True(requestBuilder?.RequestUri.AbsoluteUri == expectedPath || requestBuilder?.RequestUri.AbsoluteUri == (expectedPath + "/"));
    }

    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Headers_Class_Data))]
    public void String_WithHeader_Should_Add_Headers_Correctly(IReadOnlyCollection<KeyValuePair<string, string>> headersToAdd,
        Dictionary<string, string> expectedHeaders)
    {
        //Arrange
        var testBaseUrl = "http://localhost:5000";
        HttpRequestBuilder? requestBuilder = null;
        
        //Act
        for (int i = 0; i < headersToAdd.Count; i++)
        {
            var header = headersToAdd.ElementAt(i);
            if (i == 0)
            {
                requestBuilder = testBaseUrl.WithHeader(header.Key, header.Value);
                continue;
            } 
            requestBuilder = requestBuilder?.WithHeader(header.Key, header.Value);
        }     
        //Assert
        Assert.Equal(expectedHeaders, requestBuilder?.Headers ?? []);
    }
    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Headers_Class_Data))]
    public void Uri_WithHeader_Should_Add_Headers_Correctly(IReadOnlyCollection<KeyValuePair<string, string>> headersToAdd,
        Dictionary<string, string> expectedHeaders)
    {
        //Arrange
        var testBaseUrl = "http://localhost:5000".ToUri();
        HttpRequestBuilder? requestBuilder = null;
        
        //Act
        for (int i = 0; i < headersToAdd.Count; i++)
        {
            var header = headersToAdd.ElementAt(i);
            if (i == 0)
            {
                requestBuilder = testBaseUrl.WithHeader(header.Key, header.Value);
                continue;
            } 
            requestBuilder = requestBuilder?.WithHeader(header.Key, header.Value);
        }     
        //Assert
        Assert.Equal(expectedHeaders, requestBuilder?.Headers ?? []);
    }
    private sealed class HttpRequestBuilderUriExtensionsTests_Headers_Class_Data : TheoryData<IReadOnlyCollection<KeyValuePair<string, string>>,
        Dictionary<string, string>>
    {
        public HttpRequestBuilderUriExtensionsTests_Headers_Class_Data()
        {
            Add(
                new Dictionary<string, string> { { "Authorization", "Bearer token123" } },
                new Dictionary<string, string> { { "Authorization", "Bearer token123" } }
            );

            Add(
                new Dictionary<string, string>
                {
                    { "Authorization", "Bearer token123" },
                    { "Accept", "application/json" }
                },
                new Dictionary<string, string>
                {
                    { "Authorization", "Bearer token123" },
                    { "Accept", "application/json" }
                }
            );

            Add(
                new Dictionary<string, string> { { "authorization", "Bearer token123" } },
                new Dictionary<string, string> { { "authorization", "Bearer token123" } }
            );

            Add(
                new Dictionary<string, string> { { "X-Empty-Header", "" } },
                new Dictionary<string, string> { { "X-Empty-Header", "" } }
            );

            Add(
                new Dictionary<string, string>
                {
                    { "Header1", "Value1" },
                    { "Header2", "" }
                },
                new Dictionary<string, string>
                {
                    { "Header1", "Value1" },
                    { "Header2", "" }
                }
            );

            Add(
                new Dictionary<string, string> { { " X-Whitespace ", "  Some Value  " } },
                new Dictionary<string, string> { { " X-Whitespace ", "  Some Value  " } }
            );

            Add(
                new Dictionary<string, string> { { "X-Null-Header", null! } },
                new Dictionary<string, string> { { "X-Null-Header", null! } }
            );

            Add(
                new Dictionary<string, string>(),
                new Dictionary<string, string>()
            );

            Add(
                new Dictionary<string, string>
                {
                    { "X-Special-Header", "✓ à la mode" },
                    { "Emoji-Header", "🔥" }
                },
                new Dictionary<string, string>
                {
                    { "X-Special-Header", "✓ à la mode" },
                    { "Emoji-Header", "🔥" }
                }
            );

            Add(
                new Dictionary<string, string> { { "X-Number", "1234567890" } },
                new Dictionary<string, string> { { "X-Number", "1234567890" } }
            );

            Add(
                new List<KeyValuePair<string, string>>
                {
                    new ("Authorization", "OldToken"),
                    new ("Authorization", "NewToken"),
                },
                new Dictionary<string, string>
                {
                    { "Authorization", "NewToken" }
                }
            );

            Add(
                new Dictionary<string, string> { { "Path", "/api/v1/" } },
                new Dictionary<string, string> { { "Path", "/api/v1/" } }
            );

            Add(
                new Dictionary<string, string> { { "X-Custom_@-Header", "SomeValue" } },
                new Dictionary<string, string> { { "X-Custom_@-Header", "SomeValue" } }
            );

            Add(
                new Dictionary<string, string>
                {
                    { "Accept-Language", "en-US,en;q=0.5" },
                    { "Content-Type", "application/json" },
                    { "X-Empty", "" },
                    { "Emoji", "😎" }
                },
                new Dictionary<string, string>
                {
                    { "Accept-Language", "en-US,en;q=0.5" },
                    { "Content-Type", "application/json" },
                    { "X-Empty", "" },
                    { "Emoji", "😎" }
                }
            );

            Add(
                new Dictionary<string, string> { { "X-Long-Header", new string('a', 1000) } },
                new Dictionary<string, string> { { "X-Long-Header", new string('a', 1000) } }
            );

            Add(
                new Dictionary<string, string> { { "X-Null-String", "null" } },
                new Dictionary<string, string> { { "X-Null-String", "null" } }
            );

            Add(
                new Dictionary<string, string> { { " ", "WhitespaceKey" } },
                new Dictionary<string, string> { { " ", "WhitespaceKey" } }
            );

            Add(
                new List<KeyValuePair<string, string>>
                {
                    new ("Duplicate", "firstvalue"),
                    new ("Duplicate", "SecondValue"),
                },
                new Dictionary<string, string>
                {
                    { "Duplicate", "SecondValue" }
                }
            );
        }
    }
    private sealed class HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data : TheoryData<IReadOnlyCollection<string>, string>
    {
        public HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data()
        {
            Add(["https://www.test.com", "onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer");
            Add(["https://www.test.com/", "onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer");
            Add(["https://www.test.com/", "/onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer");
            Add(["https://www.test.com/", "/onelayer", "/twolayer"], "https://www.test.com/onelayer/twolayer");
            Add(["https://www.test.com/", "/onelayer/", "/twolayer/"], "https://www.test.com/onelayer/twolayer");

            Add(["https://www.test.com", "singlelayer"], "https://www.test.com/singlelayer");
            Add(["https://www.test.com/", "singlelayer/"], "https://www.test.com/singlelayer");
            Add(["https://www.test.com", "/singlelayer/"], "https://www.test.com/singlelayer");
            Add(["https://www.test.com/", "/singlelayer"], "https://www.test.com/singlelayer");

            Add(["https://www.test.com/api", "v1", "users"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com/api/", "v1", "users/"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com/api/", "/v1/", "/users/"], "https://www.test.com/api/v1/users");

            Add(["https://www.test.com/api", "/v1", "/users"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com/", "api", "v1", "users"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com", "/api", "/v1", "/users"], "https://www.test.com/api/v1/users");

            Add(["https://www.test.com", "api/", "/v1/", "users/"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com", "api", "", "v1", "users"], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com", "", "api", "", "v1", "", "users"], "https://www.test.com/api/v1/users");

            Add(["https://www.test.com", "", "api", "v1", "", "users", ""], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com/", "", "", "", "api", "", "v1", "users", ""], "https://www.test.com/api/v1/users");
            Add(["https://www.test.com/", " ", " ", " ", "api", " ", "v1", "users", " "], "https://www.test.com/api/v1/users");

            Add(["https://www.test.com", "api", "v1?", "users"], "https://www.test.com/api/v1?/users");
            Add(["https://www.test.com", "api", "v1", "?users"], "https://www.test.com/api/v1/?users");

            Add(["https://www.test.com", "", ""], "https://www.test.com");
            Add(["https://www.test.com/", "", ""], "https://www.test.com");

            Add(["https://www.test.com", "a", "b", "c", "d", "e"], "https://www.test.com/a/b/c/d/e");
            Add(["https://www.test.com/", "/a/", "/b/", "/c/", "/d/", "/e/"], "https://www.test.com/a/b/c/d/e");

            Add(["https://www.test.com", "////a", "////b", "////c"], "https://www.test.com/a/b/c");
            Add(["https://www.test.com/", "////a", "////b", "////c"], "https://www.test.com/a/b/c");

            Add(["https://www.test.com", "spaces are bad", "more spaces"], "https://www.test.com/spaces%20are%20bad/more%20spaces");
            Add(["https://www.test.com/", "spaces are bad/", "/more spaces/"], "https://www.test.com/spaces%20are%20bad/more%20spaces");

            Add(["https://www.test.com", "already%20encoded", "next"], "https://www.test.com/already%20encoded/next");
            Add(["https://www.test.com/", "already%20encoded/", "/next/"], "https://www.test.com/already%20encoded/next");

            Add(["https://www.test.com", "çå∞", "∆˚¬"], "https://www.test.com/%C3%A7%C3%A5%E2%88%9E/%E2%88%86%CB%9A%C2%AC");
            Add(["https://www.test.com/", "çå∞/", "/∆˚¬/"], "https://www.test.com/%C3%A7%C3%A5%E2%88%9E/%E2%88%86%CB%9A%C2%AC");

            Add(["https://www.test.com", "123", "456"], "https://www.test.com/123/456");
            Add(["https://www.test.com/", "123/", "/456/"], "https://www.test.com/123/456");

            Add(["https://www.test.com"], "https://www.test.com");
            Add(["https://www.test.com/"], "https://www.test.com");

            Add(["https://www.test.com", "", "", ""], "https://www.test.com");

            Add(["https://www.test.com///", "///onelayer///", "///twolayer///"], "https://www.test.com/onelayer/twolayer");

            Add(["https://www.test.com", "\\onelayer", "\\twolayer"], "https://www.test.com/onelayer/twolayer");
            Add(["https://www.test.com/", "\\onelayer\\", "\\twolayer\\"], "https://www.test.com/onelayer/twolayer");

            Add(["https://www.test.com", "layer1?", "layer2"], "https://www.test.com/layer1?/layer2");
            Add(["https://www.test.com", "layer1#", "layer2"], "https://www.test.com/layer1#/layer2");

            Add(["https://www.test.com", "this", "is", "a", "very", "long", "path", "to", "test"], "https://www.test.com/this/is/a/very/long/path/to/test");

            Add(["https://example.org", "path"], "https://example.org/path");
            Add(["https://example.net/", "path/"], "https://example.net/path");
        }
    }
}