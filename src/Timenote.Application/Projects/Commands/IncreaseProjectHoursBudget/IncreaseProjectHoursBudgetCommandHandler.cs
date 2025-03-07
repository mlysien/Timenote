﻿using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

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

            if (request.NewHoursBudget < project.HoursBudget)
            {
                throw new ProjectInvalidHoursBudgetException("New hours budget cannot be less than the current");
            }
            
            project.HoursBudget = request.NewHoursBudget;

            await repository.UpdateAsync(project);

            return project.Id;
        }
        catch (ProjectNotFoundException notFoundException)
        {
            return Result.Failure<Unique>(new Error(ErrorType.NotFound, notFoundException.Message));
        }
        catch (ProjectInvalidHoursBudgetException budgetHoursException)
        {
            return Result.Failure<Unique>(new Error(ErrorType.Conflict, budgetHoursException.Message));
        }
        catch (Exception exception)
        {
            return Result.Failure<Unique>(new Error(ErrorType.Failure, exception.Message));
        }
    }
}