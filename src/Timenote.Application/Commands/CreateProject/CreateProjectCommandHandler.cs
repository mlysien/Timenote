﻿using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Commands.CreateProject;

internal sealed class CreateProjectCommandHandler(IProjectRepository projectRepository)
    : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Budget = request.HoursBudget,
            IsActive = false
        };

        if (await projectRepository.CodeExistsAsync(project.Code))
        {
            return Result.Failure<Guid>(Error.Conflict("Project.CodeAlreadyExists",
                $"Project with Code: '{project.Code}' already exists"));
        }
        
        await projectRepository.AddAsync(project);
        
        return project.Id;
    }
}