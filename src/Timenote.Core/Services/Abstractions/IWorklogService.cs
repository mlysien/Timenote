using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IWorklogService
{
    void AddEntry(Entry entry);
    
    ICollection<Entry> GetEntries();
    
    ICollection<Entry> GetEntriesForDay(DateTime date);
    
    TimeSpan GetLoggedTimeForDay(DateTime date);
}