using Timenote.Domain.Entities;

namespace Timenote.Persistence.Repositories.Abstractions;

public interface IEntryRepository
{
    void Add(Entry entry);
    
    void Update(Entry entry);
    
    void Remove(Entry entry);
    
    Entry? Get(Guid id);
    
    IEnumerable<Entry> GetAll();
}