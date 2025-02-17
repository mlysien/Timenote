namespace Timenote.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; init; }
    
    public string Email { get; init; }
}