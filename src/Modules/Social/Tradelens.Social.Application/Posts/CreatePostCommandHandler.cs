using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostId>
{
    private readonly IPostRepository _repository;

    public CreatePostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<PostId> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        var post = Post.Create(command.ParentPost, command.Cashtags, command.Content, command.Sentiment);

        await _repository.CreatePost(post, cancellationToken);

        return post.Id;
    }
}