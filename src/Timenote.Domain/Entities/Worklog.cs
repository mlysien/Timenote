namespace Timenote.Domain.Entities;

/// <summary>
/// Represents Worklog
/// </summary>
public class Worklog : EntityBase
{
    public int UserId { get; init; }
    
    public User User { get; init; } = null!;
    
    public Guid ProjectId { get; init; }
    
    public Project Project { get; init; } = null!;
    
    public IReadOnlyCollection<Entry> Entries { get; init; }
}