using Timenote.Domain.Entities;
using Timenote.Persistence.Context;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Persistence.Repositories.Implementations;

public class EntryRepository(DatabaseContext context) : IEntryRepository
{
    public void Add(Entry entry)
    {
        context.Entries.Add(entry);
        context.SaveChanges();
    }
}