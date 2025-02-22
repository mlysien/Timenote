using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(Guid projectId);
    
    Task<bool> ExistsAsync(Guid projectId);
    
    Task<bool> ExistsAsync(string projectName);
    
    Task<Project> AddAsync(Project project);
    
    Task<Project> UpdateAsync(Project project);
    
    Task DeleteAsync(Project project);
}