using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public record GetPostByIdQuery(
    Guid PostId
) : IQuery<PostResponse>;