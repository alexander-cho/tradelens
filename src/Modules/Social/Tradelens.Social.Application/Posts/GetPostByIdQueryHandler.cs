using ApriumResearch.Mediator;

namespace Tradelens.Social.Application.Posts;

public sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDto>
{
    private readonly IPostReadRepository _repository;

    public GetPostByIdQueryHandler(IPostReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<PostDto> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        return await _repository.GetPostByIdAsync(query, cancellationToken);
    }
}