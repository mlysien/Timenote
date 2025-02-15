namespace Timenote.Domain.Exceptions;

public class ProjectNotFoundException(Guid projectId) : Exception($"Project with Id: {projectId} not found");