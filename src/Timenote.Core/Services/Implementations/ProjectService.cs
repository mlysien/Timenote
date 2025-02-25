using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Core.Services.Implementations;

public sealed class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    public async Task<Project> CreateProjectAsync(Project project)
    {
        return await projectRepository.AddAsync(project);
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        if (await projectRepository.ProjectExistsAsync(project.Id))
        {
            return await projectRepository.UpdateAsync(project);
        }
        
        throw new ProjectNotFoundException(project.Id);
    }

    public async Task DeleteProjectAsync(Project project)
    {
        if (await projectRepository.ProjectExistsAsync(project.Id))
        {
            await projectRepository.DeleteAsync(project);
        }
        
        throw new ProjectNotFoundException(project.Id);
    }
}