using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Core.Services.Implementations;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    public async Task CreateProjectAsync(Project project)
    {
        await projectRepository.Add(project);
    }
}