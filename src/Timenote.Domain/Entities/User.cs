namespace Timenote.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; init; }
    
    public string Email { get; init; }
    
    public string Password { get; init; }
    
    public IReadOnlyCollection<Project> Projects { get; init; }
    
    public IReadOnlyCollection<Worklog> Worklogs { get; init; }
}