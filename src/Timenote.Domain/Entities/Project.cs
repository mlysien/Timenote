namespace Timenote.Domain.Entities;

public sealed class Project : EntityBase
{
    public string Name { get; init; } = string.Empty;

    public long Budget { get; init; }

    public bool IsActive { get; init; }
    
    public IReadOnlyCollection<Worklog> Worklogs { get; init; }
}