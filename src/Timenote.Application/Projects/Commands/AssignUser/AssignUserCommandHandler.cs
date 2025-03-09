using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Projects.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(
    IProjectRepository projectRepository,
    IUserRepository userRepository
) : ICommandHandler<AssignUserCommand>
{
    public async Task<Result> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await userRepository.ExistsAsync(request.UserId))
            {
                throw new UserNotFoundException(request.UserId);
            }

            if (!await projectRepository.ProjectExistsAsync(request.ProjectId))
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var project = await projectRepository.GetByIdAsync(request.ProjectId);
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (project.User != null && project.User.Id == user.Id)
            {
                throw new UserAlreadyAssignedException(user.Id);
            }

            project.User = user;

            await projectRepository.UpdateAsync(project);

            return Result.Success();

        }
        catch (UserAlreadyAssignedException e)
        {
            return Result.Failure(new Error(ErrorType.Conflict, e.Message));
        }
        catch (Exception e)
        {
            return Result.Failure(new Error(ErrorType.Failure, e.Message));
        }
    }
}