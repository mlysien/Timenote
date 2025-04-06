using Timenote.Domain.Entities;
using Timenote.Domain.ValueObjects;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(Guid projectId);
    
    Task<bool> ExistsAsync(Unique projectId);
    
    Task<bool> CodeExistsAsync(string projectCode);
    
    Task<Project> AddAsync(Project project);
    
    Task<Project> UpdateAsync(Project project);
    
    Task DeleteAsync(Project project);
}