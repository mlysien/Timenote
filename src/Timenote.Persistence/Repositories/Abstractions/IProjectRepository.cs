using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(Guid projectId);
    
    Task<bool> CodeExistsAsync(Guid projectId);
    
    Task<bool> CodeExistsAsync(string projectCode);
    
    Task<Project> AddAsync(Project project);
    
    Task<Project> UpdateAsync(Project project);
    
    Task DeleteAsync(Project project);
}