using ApriumResearch.Mediator;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _repository;

    public CreatePostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        // consider whether this validation and converting the request/command -> valid domain aggregate/entity
        PostId? parentPostId = null;
        if (command.ParentPostId.HasValue)
        {
            parentPostId = new PostId(command.ParentPostId.Value);
        }

        var cashtags = new List<Cashtag>();
        if (command.Cashtags != null)
        {
            foreach (var ct in command.Cashtags)
            {
                var cashtag = new Cashtag(ct);
                cashtags?.Add(cashtag);
            }
            // cashtags.AddRange(command.Cashtags.Select(ct => new Cashtag(ct)));
        }

        Sentiment? sentiment = null;
        if (command.Sentiment != null)
        {
            sentiment = new Sentiment(command.Sentiment);
        }

        var post = Post.Create(parentPostId, cashtags, command.Content, sentiment);

        await _repository.CreatePost(post, cancellationToken);

        return post.Id.Value;
    }
}