using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Users.Commands.ChangeEmail;

internal sealed class ChangeEmailCommandHandler(IUserRepository userRepository)
: ICommandHandler<ChangeEmailCommand>
{
    public async Task<Result> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(request.UserId);

            if (user.Email == request.NewEmail)
            {
                return Result.Failure(new Error(ErrorType.Conflict, "Cannot change the email because is the same"));
            }

            user.Email = request.NewEmail;

            await userRepository.UpdateAsync(user);

            return Result.Success();
        }
        catch (UserNotFoundException e)
        {
            return Result.Failure(new Error(ErrorType.NotFound, e.Message));
        }
    }
}