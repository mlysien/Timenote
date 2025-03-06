namespace Timenote.Domain.Exceptions;

public class UserAlreadyAssignedException(Guid userId) 
    : Exception($"User with Id: {userId} is already assigned to project");