using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Web;
using BT.Common.Helpers.Abstract;
using BT.Common.Helpers.Models;

namespace BT.Common.Http.Models;

public sealed class HttpRequestBuilder: IValidatable
{
    public HttpMethod? HttpMethod
    {
        get;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            field = value;
        }
    }

    public Uri RequestUri
    {
        get;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            field = value;
        }
    } = null!;

    internal HttpContent? Content
    {
        get;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            field = value;
        }
    }

    internal HttpStatusCode[] AllowedHttpStatusCodes { get; set; } = [];
    internal Func<HttpResponseMessage, CancellationToken, Task<string?>>? AsyncErrorExtractor { get; set; }
    internal Dictionary<string, string> Headers { get; init; } = [];
    private bool PropertiesHaveChangedSinceLastValidation { get; set; } = false;

    public ValidationResult Validate()
    {
        var errorList = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(RequestUri.AbsoluteUri))
        {
            errorList.Add(new ValidationError
            {
                ErrorMessage = "RequestUri is required."
            });
        }

        if (HttpMethod is null)
        {
            errorList.Add(new ValidationError
            {
                ErrorMessage = "HttpMethod is required."
            });
        }

        return new ValidationResult
        {
            ValidationErrors = errorList
        };
    }
    
    [field: AllowNull, MaybeNull]
    private ValidationResult ValidationResult
    {
        get
        {
            if (field is null || PropertiesHaveChangedSinceLastValidation)
            {
                PropertiesHaveChangedSinceLastValidation = false;
                return field ??= Validate();
            }
            else
            {
                return field;
            }
        }
    }

    private Dictionary<string, string> _queryParams = [];

    internal HttpRequestBuilder(Uri requestUri)
    {
        RequestUri = requestUri;
    }
    public HttpRequestMessage ToHttpRequestMessage()
    {
        if (!IsValidRequest())
        {
            var sb = new StringBuilder();

            foreach (var error in GetRequestValidationErrors())
            {
                sb.AppendLine($"{error} ");
            }
            throw new HttpRequestException(sb.ToString().Trim());
        }

        var httpRequestMessage = new HttpRequestMessage(HttpMethod!, GetFinalUrl());

        if (Content is not null)
        {
            httpRequestMessage.Content = Content;
        }

        foreach (var header in Headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }

        return httpRequestMessage;
    }

    internal void AddAsyncErrorExtractor(Func<HttpResponseMessage, CancellationToken, Task<string?>> errorExtractor)
    {
        AsyncErrorExtractor = errorExtractor;
    }
    internal void AddHttpStatusCodes(params HttpStatusCode[] httpStatusCodes)
    {
        AllowedHttpStatusCodes = AllowedHttpStatusCodes.Length < 1 ? httpStatusCodes: AllowedHttpStatusCodes
            .ToList()
            .Concat(httpStatusCodes)
            .ToArray();
    }
    internal void AddQueryParameter(string key, string value)
    {
        PropertiesHaveChangedSinceLastValidation = true;
        _queryParams.Add(key, value);
    }

    internal void AddHeader(string name, string value)
    {
        PropertiesHaveChangedSinceLastValidation = true;
        Headers[name] = value;
    }

    internal Uri GetFinalUrl()
    {
        var uriBuilder = new UriBuilder(RequestUri);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var kvp in _queryParams)
        {
            queryParams[kvp.Key] = kvp.Value;
        }
        uriBuilder.Query = queryParams.ToString();

        return uriBuilder.Uri;
    }

    private bool IsValidRequest()
    {
        return ValidationResult.IsValid;
    }

    private string[] GetRequestValidationErrors()
    {
        return ValidationResult.ValidationErrors.Select(x => x.ErrorMessage).ToArray();
    }
}
