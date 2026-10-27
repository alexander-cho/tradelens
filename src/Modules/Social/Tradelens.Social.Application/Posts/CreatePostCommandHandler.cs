using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostId>
{
    private readonly IPostRepository _repository;

    public CreatePostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<PostId> Handle(CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        var handleCommand = _repository.CreateAsync(command, cancellationToken);
        return await handleCommand;
    }
}