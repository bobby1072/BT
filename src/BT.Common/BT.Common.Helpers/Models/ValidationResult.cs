namespace BT.Common.Helpers.Models;

public sealed record ValidationResult
{
    public bool IsValid => ValidationErrors.Count == 0;
    public IReadOnlyList<ValidationError> ValidationErrors { get; init; } = [];

    public string GetErrors() =>
        $"{string.Join(". ", ValidationErrors.Select(x => x.ErrorMessage).ToArray())}.";
}
