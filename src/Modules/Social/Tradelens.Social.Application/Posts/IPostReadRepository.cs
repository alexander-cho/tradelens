namespace Tradelens.Social.Application.Posts;

public interface IPostReadRepository
{
    Task<PostResponse> GetPostByIdAsync(GetPostByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<PostResponse>> GetPostsAsync(GetPostsQuery query, CancellationToken cancellationToken);
}