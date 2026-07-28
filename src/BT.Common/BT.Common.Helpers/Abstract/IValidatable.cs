using BT.Common.Helpers.Models;

namespace BT.Common.Helpers.Abstract;

public interface IValidatable
{
    ValidationResult Validate();
}

public interface IValidatable<in TInput>
{
    ValidationResult Validate(TInput input);
}