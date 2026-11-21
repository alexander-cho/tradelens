using Tradelens.Shared.Application;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostId>
{
    private readonly IPostRepository _repository;

    public CreatePostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<PostId> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var post = Post.Create(command.ParentPost, command.Cashtags, command.Content, command.Sentiment);

        await _repository.CreatePost(post, cancellationToken);

        return post.Id;
    }
}