namespace ApriumResearch.Mediator;

/// <summary>
/// Marker interface for commands. All concrete command types should implement this.
/// </summary>
public interface ICommand : IRequest;

/// <summary>
/// Marker interface for commands. All concrete command types should implement this.
/// </summary>
/// <remarks>
/// I recall in Vernon's DDD red book or one of his videos that a command or write is usually declared
/// with return type void, so I will have to revisit this.
/// </remarks>
/// <typeparam name="TResponse">The type of response returned by the command</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>;