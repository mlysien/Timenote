using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Projects.Commands.DecreaseProjectHoursBudget;

internal sealed class DecreaseProjectHoursBudgetCommandHandler(IProjectRepository repository) 
    : ICommandHandler<DecreaseProjectHoursBudgetCommand, Unique>
{
    public async Task<Result<Unique>> Handle(DecreaseProjectHoursBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await repository.GetByIdAsync(request.ProjectId);

            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            if (request.NewHoursBudget > project.HoursBudget)
            {
                throw new ProjectInvalidHoursBudgetException(
                    "New hours budget cannot be greater than current hours budget");
            }

            if (request.NewHoursBudget < project.BurnedHours)
            {
                throw new ProjectInvalidHoursBudgetException("New hours budget cannot be less than burned hours");
            }
            
            project.HoursBudget = request.NewHoursBudget;

            await repository.UpdateAsync(project);

            return project.Id;
        }
        catch (ProjectNotFoundException notFoundException)
        {
            return Result.Failure<Unique>(new Error("Project.NotFound", notFoundException.Message, ErrorType.NotFound));
        }
        catch (ProjectInvalidHoursBudgetException budgetHoursException)
        {
            return Result.Failure<Unique>(new Error("Project.Conflict", budgetHoursException.Message, ErrorType.Conflict));
        }
        catch (Exception exception)
        {
            return Result.Failure<Unique>(new Error("Project.Failure", exception.Message, ErrorType.Failure));
        }
    }
}