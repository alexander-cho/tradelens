using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, IEnumerable<PostResponse>>
{
    private readonly IPostReadRepository _repository;

    public GetPostsQueryHandler(IPostReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PostResponse>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetPostsAsync(request, cancellationToken);
    }
}