using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IProjectRepository
{
    Task Add(Project project);
}