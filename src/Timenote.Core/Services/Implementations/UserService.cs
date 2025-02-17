using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Core.Services.Implementations;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User> CreateUserAsync(User user)
    {
        return await userRepository.AddAsync(user);
    }
}