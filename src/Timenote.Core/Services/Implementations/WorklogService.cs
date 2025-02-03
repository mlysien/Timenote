using Timenote.Core.Services.Abstractions;
using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Implementations;

public class WorklogService : IWorklogService
{
    private readonly Worklog _worklog = new();

    public void AddEntry(Entry entry)
    {
        _worklog.Entries.Add(entry);
    }

    public ICollection<Entry> GetEntries()
    {
        return _worklog.Entries;
    }
}