﻿namespace Timenote.Domain.Entities;

public sealed class Project : EntityBase
{
    public string Code { get; init; } = string.Empty;
 
    public string Name { get; set; } = string.Empty;

    public decimal HoursBudget { get; set; }

    public decimal BurnedHours { get; set; }
    
    public bool IsActive { get; init; }
    
    public IReadOnlyCollection<Worklog> Worklogs { get; init; }
    
    public Guid UserId { get; init; }
    
    public User User { get; init; } = null!;
}