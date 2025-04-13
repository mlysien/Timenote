using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.WorkLogs.Commands.LogWorkTime;

public class LogWorkTimeCommandHandler(
    IWorkLogRepository workLogRepository,
    IUserRepository userRepository,
    IProjectRepository projectRepository)
    : ICommandHandler<LogWorkTimeCommand>
{
    public async Task<Result> Handle(LogWorkTimeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return Result.Failure(new Error(ErrorType.Conflict, "Description cannot be empty"));
            }
            
            if (request.StartTime >= request.EndTime)
            {
                return Result.Failure(new Error(ErrorType.Conflict, "Start time cannot be greater than end time"));
            }
            
            if (!await userRepository.ExistsAsync(request.UserId))
            {
                return Result.Failure(new Error(ErrorType.NotFound, 
                    $"User with Id '{request.UserId}' does not exist"));
            }

            if (!await projectRepository.ExistsAsync(request.ProjectId))
            {
                return Result.Failure(new Error(ErrorType.NotFound,
                    $"Project with Id '{request.ProjectId}' does not exist"));
            }

            await workLogRepository.AddAsync(new WorkTimeEntry()
            {
                Id = new Unique(Guid.NewGuid()),
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Description = request.Description,
            });

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure(ex.Message));
        }
    }
}