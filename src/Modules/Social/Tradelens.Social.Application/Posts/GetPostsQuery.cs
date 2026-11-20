using Tradelens.Shared.Application;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public record GetPostsQuery(
    IList<Cashtag> Cashtags,
    int PageSize
) : IQuery<IEnumerable<PostDto>>;