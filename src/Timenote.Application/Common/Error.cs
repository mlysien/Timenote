namespace Timenote.Application.Common;

public class Error(ErrorType type, string message)
{
    public ErrorType Type { get; } = type;
    
    public string Message { get; } = message;
    
    public static readonly Error None = new(ErrorType.None, string.Empty);
    public static readonly Error NullValue = new(ErrorType.Failure, string.Empty);
    
    public static Error Conflict(string description) => new(ErrorType.Conflict, description);
    
    public static Error Failure(string description) => new(ErrorType.Failure, description);
}