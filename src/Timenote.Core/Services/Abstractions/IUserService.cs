using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
}