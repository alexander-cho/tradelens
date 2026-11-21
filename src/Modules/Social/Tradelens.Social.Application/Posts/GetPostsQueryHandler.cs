using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public sealed class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostReadRepository _repository;

    public GetPostsQueryHandler(IPostReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetPostsAsync(request, cancellationToken);
    }
}