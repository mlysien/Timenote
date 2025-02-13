namespace Timenote.Domain.Entities;

public sealed class Project : EntityBase
{
    public string Name { get; init; }

    public long Budget { get; init; }

    public bool IsActive { get; init; }

    public Guid WorklogId { get; set; }
    
    public Worklog Worklog { get; init; }
}