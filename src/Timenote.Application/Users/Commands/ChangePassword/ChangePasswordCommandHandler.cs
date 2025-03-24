using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(IUserRepository userRepository) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user.Password != request.OldPassword)
            {
                return Result.Failure(new Error(ErrorType.Conflict, "Old passwords do not match"));
            }

            user.Password = request.NewPassword;

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