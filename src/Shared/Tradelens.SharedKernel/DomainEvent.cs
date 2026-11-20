namespace Tradelens.SharedKernel;

/// <summary>
/// Base class for domain events. OccuredAt property is set on creation.
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccuredAt { get; protected set; } = DateTime.UtcNow;
}