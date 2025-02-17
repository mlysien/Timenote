using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
}