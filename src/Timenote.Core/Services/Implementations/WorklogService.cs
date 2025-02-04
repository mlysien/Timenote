using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;
using Timenote.Domain.Exceptions;

namespace Timenote.Core.Services.Implementations;

public class WorklogService : IWorklogService
{
    private readonly Worklog _worklog = new();

    public void AddEntry(Entry entry)
    {
        if (entry.StartTime >= entry.EndTime)
        {
            throw new InvalidWorklogEntryException("StartTime can't be greater than EndTime");
        }
        
        _worklog.Entries.Add(entry);
    }

    public ICollection<Entry> GetEntries()
    {
        return _worklog.Entries;
    }

    public ICollection<Entry> GetEntriesForDay(DateTime date)
    {
        return _worklog.Entries.Where(entry => entry.StartTime.Date == date && entry.EndTime.Date == date).ToList();
    }

    public TimeSpan GetLoggedTimeForDay(DateTime date)
    {
        var ticks = _worklog.Entries.Sum(entry => (entry.EndTime - entry.StartTime).Ticks);
        
        return TimeSpan.FromTicks(ticks);
    }
}