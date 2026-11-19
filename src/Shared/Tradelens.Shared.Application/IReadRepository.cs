namespace Tradelens.Shared.Application;

/// <summary>
/// Marker interface for read repositories.
/// </summary>
/// <remarks>
/// As queries do not go through the domain for state validity check nor changes,
/// they are an application layer concern.
/// </remarks>
public interface IReadRepository;