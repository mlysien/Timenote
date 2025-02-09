using Microsoft.EntityFrameworkCore;
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

    public void Update(Entry entry)
    {
        context.Update(entry);
        context.SaveChanges();
    }

    public void Remove(Entry entry)
    {
        context.Remove(entry);
        context.SaveChanges();
    }

    public Entry? Get(Guid id)
    {
        return context.Entries.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Entry> GetAll()
    {
        return context.Entries.AsNoTracking();
    }
}