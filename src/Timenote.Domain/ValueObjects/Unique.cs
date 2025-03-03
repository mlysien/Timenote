namespace Timenote.Domain.ValueObjects;

/// <summary>
/// Unique identifier for entities
/// </summary>
public record Unique(Guid Identifier)
{
    public static implicit operator Guid(Unique unique) => unique.Identifier;
    
    public override string ToString()
    {
        return Identifier.ToString();
    }
}