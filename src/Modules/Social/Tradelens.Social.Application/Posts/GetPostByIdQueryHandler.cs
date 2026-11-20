using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDto>
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