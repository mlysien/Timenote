namespace Timenote.Application.Common;

public class Error(ErrorType type, string description)
{
    public ErrorType Type { get; } = type;
    
    public string Description { get; } = description;
    
    public static readonly Error None = new(ErrorType.None, string.Empty);
    public static readonly Error NullValue = new(ErrorType.Failure, string.Empty);
    
    public static Error Conflict(string description) => new(ErrorType.Conflict, description);
}