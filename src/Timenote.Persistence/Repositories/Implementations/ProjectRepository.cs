using Microsoft.EntityFrameworkCore;
using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

internal sealed class ProjectRepository(DatabaseContext context) : IProjectRepository
{
    public async Task<Project> AddAsync(Project project)
    {
        context.Projects.Add(project);
        
        await context.SaveChangesAsync();
        
        return project;
    }
}