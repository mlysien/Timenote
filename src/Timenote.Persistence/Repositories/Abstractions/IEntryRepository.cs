using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IEntryRepository
{
    void Add(Entry entry);
}