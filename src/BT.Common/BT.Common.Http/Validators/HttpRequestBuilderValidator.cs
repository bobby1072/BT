using System.Diagnostics.CodeAnalysis;
using BT.Common.Http.Models;
using FluentValidation;

namespace BT.Common.Http.Validators;

[ExcludeFromCodeCoverage]
internal class HttpRequestBuilderValidator: AbstractValidator<HttpRequestBuilder>
{
    private HttpRequestBuilderValidator()
    {
        RuleFor(x => x.RequestUri).NotNull().WithMessage("Request uri has not been specified is required.");
        RuleFor(x => x.RequestUri).NotEmpty().WithMessage("Request uri has not been specified is required.");
        RuleFor(x => x.HttpMethod).NotNull().WithMessage("HttpMethod is required.");
    }
    
    public static readonly HttpRequestBuilderValidator DefaultValidator = new();
}