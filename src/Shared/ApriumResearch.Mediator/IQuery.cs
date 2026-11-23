namespace ApriumResearch.Mediator;

/// <summary>
/// Marker interface for queries. All concrete query types should implement this.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the query</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>;