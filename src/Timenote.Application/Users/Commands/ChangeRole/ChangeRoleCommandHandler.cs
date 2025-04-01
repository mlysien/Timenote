using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Users.Commands.ChangeRole;

internal sealed class ChangeRoleCommandHandler(IUserRepository userRepository) : ICommandHandler<ChangeRoleCommand>
{
    public async Task<Result> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            
            user.Role = request.NewRole;

            await userRepository.UpdateAsync(user);

            return Result.Success();
        }
        catch (UserNotFoundException exception)
        {
            return Result.Failure(new Error(ErrorType.NotFound, exception.Message));
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.Failure, exception.Message));
        }
    }
}