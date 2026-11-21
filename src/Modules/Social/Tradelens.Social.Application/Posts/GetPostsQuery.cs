using Tradelens.Shared.Application;
// using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public sealed record GetPostsQuery(
    // // select from existing cashtags dropdown
    // // search bar
    // // main ticker page feed, etc.
    // IList<Cashtag> Cashtags,
    int? PageSize
) : IQuery<IEnumerable<PostDto>>;