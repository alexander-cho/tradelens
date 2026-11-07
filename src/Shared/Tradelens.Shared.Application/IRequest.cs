namespace Tradelens.Shared.Application;

/*
 * IRequest (Message contract). The Producer: A Request or any type implementing IRequest will be dispatched,
 * then handled by exactly one consumer and returns a result/response or void.
 */

/// <summary>
/// Marker interface to represent a Request object with a void response.
/// </summary>
public interface IRequest;

/// <summary>
/// Marker interface to represent a Request object with a response.
/// </summary>
/// <remarks>
/// With the TResponse generic type marked as covariant with the out keyword, types implementing IRequest
/// can have a type parameter that is more derived than TResponse.
///
/// For example, declare an ICommand{TResponse} type that extends this IRequest{TResponse}.
/// Then a CreatePostCommand that will also implement IRequest{Guid (or concrete type)}
/// with a more derived type than TResponse where the compiler would expect the return type to be
/// TResponse as well otherwise, but Guid/PostId is a subtype of TResponse.
/// </remarks>
/// <typeparam name="TResponse">Response generic type</typeparam>
public interface IRequest<out TResponse>;
