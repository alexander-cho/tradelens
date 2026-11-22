using Tradelens.Shared.Application;

namespace Tradelens.Social.Application.Posts;

public sealed record CreatePostCommand(
    Guid? ParentPostId,
    IList<string>? Cashtags,
    string Content,
    string? Sentiment
) : ICommand<Guid>;