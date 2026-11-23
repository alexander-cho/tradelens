using ApriumResearch.SharedKernel;

namespace Tradelens.Social.Domain.Posts;

public interface IPostRepository : IRepository<Post>
{
    Task CreatePost(Post post, CancellationToken cancellationToken);
    // Task Delete();
}