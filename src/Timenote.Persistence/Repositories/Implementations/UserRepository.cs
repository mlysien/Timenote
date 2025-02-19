using Microsoft.EntityFrameworkCore;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

internal sealed class UserRepository(DatabaseContext context) : IUserRepository
{
    public async Task<User> AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        return user;
    }

    public async Task<bool> ExistsAsync(Guid userId)
    {
        return await context.Users.AnyAsync(user => user.Id == userId);
    }

    public async Task<User> UpdateAsync(User user)
    {
        context.Users.Update(user);
        
        await context.SaveChangesAsync();
        
        return user;
    }

    public async Task RemoveAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user != null) context.Users.Remove(user);

        await context.SaveChangesAsync();
    }
}