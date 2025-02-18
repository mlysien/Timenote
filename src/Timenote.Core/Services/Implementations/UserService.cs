using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Core.Services.Implementations;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User> CreateUserAsync(User user)
    {
        return await userRepository.AddAsync(user);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        if (await userRepository.ExistsAsync(user.Id))
        {
            return await userRepository.UpdateAsync(user);
        }

        throw new UserNotFoundException(user.Id);
    }
}