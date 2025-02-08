using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IWorklogService
{
    void AddWorklogEntry(Entry entry);
    
    void UpdateEntry(Entry entry);
    
    ICollection<Entry> GetEntries();

    TimeSpan GetEntriesFromDay(DateTime day);
}