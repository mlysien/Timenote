using Microsoft.EntityFrameworkCore;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

internal sealed class ProjectRepository(DatabaseContext context) : IProjectRepository
{
    public async Task<Project> GetByIdAsync(Guid projectId)
    {
        return await context.Projects.FirstAsync(p => p.Id == projectId);
    }

    public async Task<bool> ExistsAsync(Guid projectId)
    {
        return await context.Projects.AnyAsync(p => p.Id == projectId);
    }

    public async Task<Project> AddAsync(Project project)
    {
        context.Projects.Add(project);
        
        await context.SaveChangesAsync();
        
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        context.Projects.Update(project);
        
        await context.SaveChangesAsync();
        
        return project;
    }
}