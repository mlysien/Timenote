using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    
    Task<User> UpdateUserAsync(User user);
    
    Task RemoveUserAsync(User user);
}