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

    internal Uri RequestUri
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

    internal void AddHeader(string name, string value)
    {
        PropertiesHaveChangedSinceLastValidation = true;
        Headers[name] = value;
    }
    
    internal bool IsValidRequest()
    {
        return ValidationResult.IsValid;
    }

    internal IReadOnlyCollection<string> GetRequestValidationErrors()
    {
        return ValidationResult.Errors.Select(x => x.ErrorMessage).ToArray();
    }

    internal HttpRequestBuilder(Uri requestUri)
    {
        RequestUri = requestUri;
    }
    
    private bool PropertiesHaveChangedSinceLastValidation { get; set; } = false;
    private ValidationResult? _currentValidationResult;
    private ValidationResult ValidationResult
    {
        get
        {
            if (_currentValidationResult is null || PropertiesHaveChangedSinceLastValidation)
            {
                PropertiesHaveChangedSinceLastValidation = false;
                return _currentValidationResult ??= HttpRequestBuilderValidator.DefaultValidator.Validate(this);
            }
            else
            {
                return _currentValidationResult;
            }
        }
    }
}