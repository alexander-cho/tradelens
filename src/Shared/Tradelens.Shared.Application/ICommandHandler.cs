namespace Tradelens.Shared.Application;

/// <summary>
/// Marker interface for command handlers that don't return a response
/// </summary>
/// <remarks>
/// Reacts to and executes associating command. "in" keyword from input type parameter TCommand declared as
/// contravariant, where classes implementing ICommandHandler can take in a type that is lower than TCommand,
/// as long as it implements the base ICommand, through the "where" keyword and declaration.
/// </remarks>
/// <typeparam name="TCommand">The type of command to handle</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;

/// <summary>
/// Marker interface for command handlers that return a response.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle</typeparam>
/// <typeparam name="TResponse">The type of response to be returned by the command</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;