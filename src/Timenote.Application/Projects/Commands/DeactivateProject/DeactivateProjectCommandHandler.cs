﻿using Timenote.Application.Common;
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
        try
        {
            if (!await projectRepository.ExistsAsync(request.ProjectId))
            {
                return Result.Failure<Unique>(new Error(ErrorType.NotFound,
                    $"Project with id: {request.ProjectId} does not exist"));
            }
        
            var project = await projectRepository.GetByIdAsync(request.ProjectId);

            if (project.IsActive is false)
            {
                return Result.Failure<Unique>(new Error(ErrorType.Conflict,
                    $"Project with id: {request.ProjectId} has already been deactivated"));
            }
        
            project.IsActive = false;
        
            await projectRepository.UpdateAsync(project);
        
            return project.Id;
        }
        catch (Exception e)
        {
            return Result.Failure<Unique>(new Error(ErrorType.Failure, e.Message));
        }
    }
}