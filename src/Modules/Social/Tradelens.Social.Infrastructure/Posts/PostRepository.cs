using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Infrastructure.Posts;

public class PostRepository : IPostRepository
{
    private readonly SocialDbContext _dbContext;

    public PostRepository(SocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePost(Post post, CancellationToken cancellationToken)
    {
        await _dbContext.Posts.AddAsync(post, cancellationToken);
    }
}