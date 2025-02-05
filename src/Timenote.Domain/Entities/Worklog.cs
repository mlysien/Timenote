namespace Timenote.Domain.Entities;

/// <summary>
/// Represents Worklog
/// </summary>
public class Worklog : EntityBase
{
    public List<Entry> Entries { get; private set; } = new();
}