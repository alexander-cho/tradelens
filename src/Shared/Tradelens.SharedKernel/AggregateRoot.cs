namespace Tradelens.SharedKernel;

/// <summary>
/// Repositories, or rather the respective interfaces should implement AggregateRoot.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<object> _domainEvents = new();

    protected IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}