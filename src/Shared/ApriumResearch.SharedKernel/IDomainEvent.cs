namespace ApriumResearch.SharedKernel;

/// <summary>
/// Interface for domain events.
/// </summary>
public interface IDomainEvent
{
    DateTime OccuredAt { get; }
}