using Timenote.Common.ValueObjects;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.IncreaseProjectHoursBudget;

internal sealed class IncreaseProjectHoursBudgetCommandHandler(IProjectRepository repository) 
    : ICommandHandler<IncreaseProjectHoursBudgetCommand, Unique>
{
    public async Task<Result<Unique>> Handle(IncreaseProjectHoursBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await repository.GetByIdAsync(request.ProjectId);

            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            project.HoursBudget = request.NewHoursBudget;
        
            await repository.UpdateAsync(project);
        
            return project.Id;
        }
        catch (ProjectNotFoundException notFoundException)
        {
            return Result.Failure<Unique>(new Error("Project.NotFound", notFoundException.Message, ErrorType.NotFound));
        }
    }
}