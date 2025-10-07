using System.Text;
using BT.Common.Http.Exceptions;
using BT.Common.Http.Validators;
using FluentValidation.Results;

namespace BT.Common.Http.Models;

public sealed class HttpRequestBuilder
{
    private HttpMethod? _httpMethod;
    internal HttpMethod? HttpMethod
    {
        get => _httpMethod;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            _httpMethod = value;
        }
    }

    private Uri _uri = null!;

    public Uri RequestUri
    {
        get => _uri;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            _uri = value;
        }
    }
    private HttpContent? _httpContent;

    internal HttpContent? Content
    {
        get => _httpContent;
        set
        {
            PropertiesHaveChangedSinceLastValidation = true;
            _httpContent = value;
        }
    }
    internal Dictionary<string, string> Headers { get; init; } = [];
    private bool PropertiesHaveChangedSinceLastValidation { get; set; } = false;
    private ValidationResult? _currentValidationResult;
    private ValidationResult ValidationResult
    {
        get
        {
            if (_currentValidationResult is null || PropertiesHaveChangedSinceLastValidation)
            {
                PropertiesHaveChangedSinceLastValidation = false;
                return _currentValidationResult ??=
                    HttpRequestBuilderValidator.DefaultValidator.Validate(this);
            }
            else
            {
                return _currentValidationResult;
            }
        }
    }

    private Dictionary<string, string> _queryParams = [];

    internal HttpRequestBuilder(Uri requestUri)
    {
        RequestUri = requestUri;
    }

    internal HttpRequestMessage ToHttpRequestMessage()
    {
        if (!IsValidRequest())
        {
            var sb = new StringBuilder();

            foreach (var error in GetRequestValidationErrors())
            {
                sb.AppendLine($"{error} ");
            }
            throw new HttpRequestBuilderException(sb.ToString().Trim());
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
        var queryParams = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
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
        return ValidationResult.Errors.Select(x => x.ErrorMessage).ToArray();
    }
}
