using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public interface IPostRepository
{
    Task<PostId> CreateAsync(CreatePostCommand command, CancellationToken cancellationToken);
    // Task Delete();
}