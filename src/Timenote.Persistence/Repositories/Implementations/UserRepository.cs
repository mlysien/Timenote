using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }
}