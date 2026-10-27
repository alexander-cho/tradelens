namespace Tradelens.Social.Application.Posts;

/// <summary>
/// This file will contain classes and interfaces that relate to the building blocks of an Application layer,
/// and will likely move into a Shared project that modules will reference. This includes but is not limited to
/// CQRS, custom dispatchers, etc.
/// </summary>
public class TemporarySharedApplication;

public interface IQuery<TResponse>;

public interface ICommand;

/// <summary>
/// I recall in Vernon's DDD red book that a command or write is usually declared with return type void,
/// so I will have to revisit this.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse>;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

/// <summary>
/// Reacts to and executes associating command. "in" keyword from input type parameter TCommand declared as
/// contravariant, where classes implementing ICommandHandler can take in a type that is lower than TCommand,
/// as long as it implements the base ICommand, through the "where" keyword and declaration.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}

// public interface IQueryDispatcher;
//
// public interface ICommandDispatcher;