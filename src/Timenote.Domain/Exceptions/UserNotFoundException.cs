namespace Timenote.Domain.Exceptions;

public class UserNotFoundException(Guid userId) : Exception($"User with Id: {userId} not found");