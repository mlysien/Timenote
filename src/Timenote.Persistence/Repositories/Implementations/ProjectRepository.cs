using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

public class ProjectRepository(DatabaseContext context) : IProjectRepository
{
    public async Task Add(Project project)
    {
        context.Add(project);
        
        await context.SaveChangesAsync();
    }
}