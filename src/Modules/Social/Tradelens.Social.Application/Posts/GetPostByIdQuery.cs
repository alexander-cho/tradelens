using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public sealed record GetPostByIdQuery(
    Guid PostId
) : IQuery<PostDto>;