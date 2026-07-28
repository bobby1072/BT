namespace BT.Common.Helpers.Models;

public sealed record ValidationError
{
    public required string ErrorMessage { get; init; }
}