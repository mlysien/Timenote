using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;
using Timenote.Shared.Common;
using Timenote.Shared.Messaging;

namespace Timenote.Application.Projects.Commands.AssignUser;

internal sealed class AssignUserCommandHandler(
    IProjectRepository projectRepository,
    IUserRepository userRepository
) : ICommandHandler<AssignUserCommand>
{
    public async Task<Result> Handle(AssignUserCommand request, CancellationToken cancellationToken)
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
        project.User = user;
        
        await projectRepository.UpdateAsync(project);

        return Result.Success();
    }
}