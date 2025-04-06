using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IWorkLogRepository
{
    Task AddAsync(WorkTimeEntry entry);
}