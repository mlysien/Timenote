namespace Timenote.Domain.Entities;

/// <summary>
/// Represents single Entry of the Worklog
/// </summary>
public sealed class Entry : EntityBase
{
    public DateTime StartTime { get; init; }
    
    public DateTime EndTime { get; init; }

    public Guid ProjectId { get; init; }
}