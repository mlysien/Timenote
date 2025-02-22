using Timenote.Domain.Entities;
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
            Name = request.Name,
            Budget = request.HoursBudget,
            IsActive = false
        };

        if (await projectRepository.ExistsAsync(project.Name))
        {
            // todo add specified error 
            return Result.Failure<Guid>(Error.None);
        }
        
        await projectRepository.AddAsync(project);
        
        return project.Id;
    }
}