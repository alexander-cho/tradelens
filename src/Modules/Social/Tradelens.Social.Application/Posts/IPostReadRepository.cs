using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

/// <summary>
/// Read repository interface for querying from Post aggregate
/// </summary>
public interface IPostReadRepository : IReadRepository
{
    Task<PostDto> GetPostByIdAsync(GetPostByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<PostDto>> GetPostsAsync(GetPostsQuery query, CancellationToken cancellationToken);
}