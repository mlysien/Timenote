namespace Timenote.Domain.Exceptions;

public class InvalidWorklogEntryException(string message) : Exception($"Invalid worklog entry: {message}");