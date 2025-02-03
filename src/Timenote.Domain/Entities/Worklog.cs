namespace Timenote.Domain.Entities;

/// <summary>
/// Represents Worklog
/// </summary>
public class Worklog : EntityBase
{
    public ICollection<Entry> Entries { get; private set; } = new List<Entry>();
    
    public TimeSpan LoggedTime => TimeSpan.FromTicks(Entries.Sum(entry => (entry.EndTime - entry.StartTime).Ticks));
    
    public void AddEntry(Entry entry)
    {
        if (entry.StartTime >= entry.EndTime)
        {
            throw new ArgumentException("The entry starting time must be before the entry end time.");
        }
        
        Entries.Add(entry);
    }
}