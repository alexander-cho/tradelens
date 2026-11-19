namespace Tradelens.Shared.Application;

/*
 * IRequestHandler (Message handler contract). The Consumer
 */

/// <summary>
/// Handler for requests that don't return a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Handler for requests that return a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response</typeparam>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}