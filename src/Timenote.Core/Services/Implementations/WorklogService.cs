using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;
using Timenote.Persistence.Repositories.Abstractions;

namespace Timenote.Core.Services.Implementations;

public class WorklogService(
    IEntryRepository entryRepository
) : IWorklogService
{
    public void AddWorklogEntry(Entry entry)
    {
        if (entry is null)
        {
            throw new InvalidWorklogEntryException("Entry cannot be null");
        }
        
        if (entry.StartTime == DateTime.MinValue || entry.StartTime == DateTime.MaxValue ||
            entry.EndTime == DateTime.MinValue || entry.EndTime == DateTime.MaxValue)
        {
            throw new InvalidWorklogEntryException("StartTime and EndTime must be set");
        }
        
        if (entry.StartTime >= entry.EndTime)
        {
            throw new InvalidWorklogEntryException("StartTime can't be greater than EndTime");
        }

        if (entry.ProjectId == Guid.Empty)
        {
            throw new InvalidWorklogEntryException("ProjectId cannot be empty");
        }

        var entries = entryRepository.GetAll().Where(e => e.StartTime.Date == entry.StartTime.Date);

        if (entries.Any(entryEntry => entry.StartTime >= entryEntry.StartTime || entry.StartTime <= entryEntry.EndTime))
        {
            throw new InvalidWorklogEntryException("Time can't overlap on existing entry");
        }
        
        entryRepository.Add(entry);
    }

    public void UpdateWorklogEntry(Entry entry)
    {
        var entryEntity = entryRepository.Get(entry.Id);

        if (entryEntity is not null)
        {
            entryRepository.Update(entry);
        }
    }

    public void RemoveWorklogEntry(Entry entry)
    {
        entryRepository.Remove(entry);
    }

    public ICollection<Entry> GetEntries()
    {
        return entryRepository.GetAll().ToList();
    }

    public TimeSpan GetLoggedTimeFromDay(DateTime day)
    {
        var entries = entryRepository.GetAll().Where(e => e.StartTime.Date == day.Date);
        
        var ticks = entries.Sum(entry => (entry.EndTime - entry.StartTime).Ticks);
        
        return TimeSpan.FromTicks(ticks);
    }
}