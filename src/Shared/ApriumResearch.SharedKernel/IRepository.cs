namespace ApriumResearch.SharedKernel;

/// <summary>
/// Interface for repositories.
/// </summary>
/// <typeparam name="T">Generic type parameter should extend AggregateRoot</typeparam>
public interface IRepository<T> where T : IAggregateRoot;