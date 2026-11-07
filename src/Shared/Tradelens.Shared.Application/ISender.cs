namespace Tradelens.Shared.Application;

/// <summary>
/// Sender interface responsible for dispatching requests to their corresponding handlers.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Send a request that does not return a response.
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Send(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a request that returns a response.
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResponse">The type of response returned by the request</typeparam>
    /// <returns></returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
