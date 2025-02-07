using Timenote.Domain.Entities;

namespace Timenote.Core.Services.Abstractions;

public interface IWorklogService
{
    void AddEntry(Entry entry);
    
    void UpdateEntry(Entry entry);
    
    ICollection<Entry> GetEntries();

    TimeSpan GetLoggedTimeForDay(DateTime date);
}