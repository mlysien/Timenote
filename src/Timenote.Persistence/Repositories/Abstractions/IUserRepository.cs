using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    
    Task<bool> ExistsAsync(Guid userId);
    
    Task<User> UpdateAsync(User user);
    
    Task RemoveAsync(Guid userId);
    
    Task<User> GetByIdAsync(Guid userId);
}