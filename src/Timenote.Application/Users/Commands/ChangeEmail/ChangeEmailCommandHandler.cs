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
        var user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        user.Email = request.NewEmail;
        
        await userRepository.UpdateAsync(user);
        
        return Result.Success();
    }
}