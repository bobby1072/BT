
using BT.Common.Http.Extensions;
using BT.Common.Http.Models;

namespace BT.Common.Http.Tests.ExtensionsTests;

public class HttpRequestBuilderUriExtensionsTests
{

    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data))]
    public void String_AppendPathSegment_Should_Build_Uris_Correctly(IReadOnlyCollection<string> splitPath,
        string expectedPath)
    {
        var startingBaseUrlString = splitPath.First();
        HttpRequestBuilder? requestBuilder = null;
        foreach (var segment in splitPath)
        {
            if (startingBaseUrlString == segment)
            {
                requestBuilder = startingBaseUrlString.ToUri().ToHttpRequestBuilder();
                continue;
            }
            requestBuilder = requestBuilder?.AppendPathSegment(segment);
            
        }
        Assert.Equal(expectedPath, requestBuilder?.RequestUri.AbsoluteUri);
    }
    [Theory]
    [ClassData(typeof(HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data))]
    public void Uri_AppendPathSegment_Should_Build_Uris_Correctly(IReadOnlyCollection<string> splitPath,
        string expectedPath)
    {
        var startingBaseUrlUri = new Uri(splitPath.First());
        HttpRequestBuilder? requestBuilder = null;
        foreach (var segment in splitPath)
        {
            if (startingBaseUrlUri.AbsoluteUri == segment)
            {
                requestBuilder = startingBaseUrlUri.ToHttpRequestBuilder();
                continue;
            }
            requestBuilder = requestBuilder?.AppendPathSegment(segment);
        }
        
        Assert.Equal(expectedPath, requestBuilder?.RequestUri.AbsoluteUri);
    }
    
    private class HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data : TheoryData<IReadOnlyCollection<string>, string>
    {
        public HttpRequestBuilderUriExtensionsTests_Endpoints_Class_Data()
        {
            Add(["https://www.test.com", "onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer/");
            Add(["https://www.test.com/", "onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer/");
            Add(["https://www.test.com/", "/onelayer", "twolayer"], "https://www.test.com/onelayer/twolayer/");
            Add(["https://www.test.com/", "/onelayer", "/twolayer"], "https://www.test.com/onelayer/twolayer/");
            Add(["https://www.test.com/", "/onelayer/", "/twolayer/"], "https://www.test.com/onelayer/twolayer/");

            Add(["https://www.test.com", "singlelayer"], "https://www.test.com/singlelayer/");
            Add(["https://www.test.com/", "singlelayer/"], "https://www.test.com/singlelayer/");
            Add(["https://www.test.com", "/singlelayer/"], "https://www.test.com/singlelayer/");
            Add(["https://www.test.com/", "/singlelayer"], "https://www.test.com/singlelayer/");

            Add(["https://www.test.com/api", "v1", "users"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com/api/", "v1", "users/"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com/api/", "/v1/", "/users/"], "https://www.test.com/api/v1/users/");

            Add(["https://www.test.com/api", "/v1", "/users"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com/", "api", "v1", "users"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com", "/api", "/v1", "/users"], "https://www.test.com/api/v1/users/");

            Add(["https://www.test.com", "api/", "/v1/", "users/"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com", "api", "", "v1", "users"], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com", "", "api", "", "v1", "", "users"], "https://www.test.com/api/v1/users/");

            Add(["https://www.test.com", "", "api", "v1", "", "users", ""], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com/", "", "", "", "api", "", "v1", "users", ""], "https://www.test.com/api/v1/users/");
            Add(["https://www.test.com/", " ", " ", " ", "api", " ", "v1", "users", " "], "https://www.test.com/api/v1/users/");

            Add(["https://www.test.com", "api", "v1?", "users"], "https://www.test.com/api/v1?/users?");
            Add(["https://www.test.com", "api", "v1", "?users"], "https://www.test.com/api/v1/?users?");

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

            Add(["https://www.test.com", "çå∞", "∆˚¬"], "https://www.test.com/çå∞/∆˚¬");
            Add(["https://www.test.com/", "çå∞/", "/∆˚¬/"], "https://www.test.com/çå∞/∆˚¬");

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