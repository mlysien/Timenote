using Timenote.Domain.Enums;

namespace Timenote.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; init; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }

    public UserRole Role { get; set; }
    
    public IReadOnlyCollection<Project> Projects { get; init; }
    
    public IReadOnlyCollection<Worklog> Worklogs { get; init; }
}