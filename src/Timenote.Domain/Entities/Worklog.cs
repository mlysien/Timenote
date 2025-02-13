namespace Timenote.Domain.Entities;

/// <summary>
/// Represents Worklog
/// </summary>
public class Worklog : EntityBase
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public List<Entry> Entries { get; private set; } = new();
}