using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    
    Task<bool> ExistsAsync(Unique userId);
    
    Task<bool> EmailExistsAsync(string email);
    
    Task<User> UpdateAsync(User user);
    
    Task RemoveAsync(Guid userId);
    
    Task<User> GetByIdAsync(Guid userId);
}