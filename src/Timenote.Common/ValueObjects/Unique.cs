namespace Timenote.Common.ValueObjects;

/// <summary>
/// Unique identifier for entities
/// </summary>
public record Unique(Guid Value)
{
    public static implicit operator Guid(Unique unique) => unique.Value;
    
    public override string ToString()
    {
        return Value.ToString();
    }
}