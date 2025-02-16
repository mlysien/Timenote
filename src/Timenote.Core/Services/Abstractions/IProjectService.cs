using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IProjectService
{
    Task<Project> CreateProjectAsync(Project project);
    
    Task<Project> UpdateProjectAsync(Project project);
    
    Task DeleteProjectAsync(Project project);
}