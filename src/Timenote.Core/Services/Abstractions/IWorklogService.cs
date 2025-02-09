using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IWorklogService
{
    void AddWorklogEntry(Entry entry);
    
    void UpdateWorklogEntry(Entry entry);
    
    void RemoveWorklogEntry(Entry entry);
    
    ICollection<Entry> GetEntries();

    TimeSpan GetLoggedTimeFromDay(DateTime day);
}