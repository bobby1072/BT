using BT.Common.Helpers.Models;

namespace BT.Common.Helpers.Abstract;

public interface IAsyncValidatable
{
    Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default);
}

public interface IAsyncValidatable<in TInput>
{
    Task<ValidationResult> ValidateAsync(TInput input, CancellationToken cancellationToken = default);
}