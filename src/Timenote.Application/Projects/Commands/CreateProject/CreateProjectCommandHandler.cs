using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Projects.Commands.CreateProject;

internal sealed class CreateProjectCommandHandler(IProjectRepository projectRepository)     
    : ICommandHandler<CreateProjectCommand, Unique>
{
    public async Task<Result<Unique>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = new Project
            {
                Id = new Unique(Guid.NewGuid()),
                Code = request.Code,
                Name = request.Name,
                HoursBudget = request.HoursBudget,
                IsActive = false
            };

            if (await projectRepository.CodeExistsAsync(project.Code))
            {
                return Result.Failure<Unique>(Error.Conflict("Project.CodeAlreadyExists",
                    $"Project with Code: '{project.Code}' already exists"));
            }
        
            await projectRepository.AddAsync(project);
        
            return project.Id;
        }
        catch (Exception e)
        {
            return Result.Failure<Unique>(new Error("Project.Failure", e.Message, ErrorType.Failure));
        }
    }
}