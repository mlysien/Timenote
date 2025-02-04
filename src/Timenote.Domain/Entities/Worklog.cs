namespace Timenote.Domain.Entities;

/// <summary>
/// Represents Worklog
/// </summary>
public class Worklog : EntityBase
{
    public ICollection<Entry> Entries { get; private set; } = new List<Entry>();
}