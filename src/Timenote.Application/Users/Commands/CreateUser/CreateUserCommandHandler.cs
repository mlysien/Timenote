using Timenote.Application.Common;
using Timenote.Application.Messaging;
using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(IUserRepository userRepository) : ICommandHandler<CreateUserCommand, Unique>
{
    public async Task<Result<Unique>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var emailExist = await userRepository.EmailExistsAsync(request.Email);

        if (emailExist)
        {
            return Result.Failure<Unique>(Error.Conflict($"Email '{request.Email}' already exists"));
        }

        var user = new User
        {
            Id = new Unique(Guid.NewGuid()),
            Email = request.Email,
            Name = request.Name,
            Password = request.Password
        };
        
        await userRepository.AddAsync(user);

        return user.Id;
    }
}