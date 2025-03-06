namespace Timenote.Application.Common;

public class Error(string code, string description, ErrorType type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public string Code { get; } = code;

    public string Description { get; } = description;

    public ErrorType Type { get; } = type;

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
}