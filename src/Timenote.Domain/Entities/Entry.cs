namespace Timenote.Domain.Entities;

/// <summary>
/// Represents single Entry of the Worklog
/// </summary>
public sealed class Entry : EntityBase
{
    public DateTime StartTime { get; init; }
    
    public DateTime EndTime { get; init; }
    
    public Guid  WorklogId { get; init; }
    
    public Worklog Worklog { get; init; } = null!;
}