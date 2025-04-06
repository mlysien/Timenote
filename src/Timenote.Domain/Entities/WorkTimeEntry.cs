using Timenote.Domain.ValueObjects;

namespace Timenote.Domain.Entities;

public sealed class WorkTimeEntry : EntityBase
{
    public Unique UserId { get; set; }

    public Unique ProjectId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Description { get; set; }
}