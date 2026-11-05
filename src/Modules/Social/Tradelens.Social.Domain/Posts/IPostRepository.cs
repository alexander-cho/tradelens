namespace Tradelens.Social.Domain.Posts;

public interface IPostRepository
{
    Task CreatePost(Post post, CancellationToken cancellationToken);
    // Task Delete();
}