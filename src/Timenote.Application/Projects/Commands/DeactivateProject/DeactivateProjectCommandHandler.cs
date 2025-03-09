using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Projects.Commands.DeactivateProject;

internal sealed class DeactivateProjectCommandHandler(
    IProjectRepository projectRepository
) : ICommandHandler<DeactivateProjectCommand, Unique>
{
    public async Task<Result<Unique>> Handle(DeactivateProjectCommand request, CancellationToken cancellationToken)
    {
        if (!await projectRepository.ProjectExistsAsync(request.ProjectId))
        {
            return Result.Failure<Unique>(new Error(ErrorType.NotFound,
                $"Project with id: {request.ProjectId} does not exist"));
        }
        
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        
        project.IsActive = false;
        
        await projectRepository.UpdateAsync(project);
        
        return project.Id;
    }
}