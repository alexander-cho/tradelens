using Tradelens.Social.Application.Posts;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Infrastructure.Posts;

public class PostRepository : IPostRepository
{
    private readonly SocialDbContext _dbContext;

    public PostRepository(SocialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PostId> CreateAsync(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var commandToObj = Post.Create(command.ParentPost, command.Cashtags, command.Content, command.Sentiment);
        await _dbContext.Posts.AddAsync(commandToObj, cancellationToken);
        return commandToObj.Id;
    }
}