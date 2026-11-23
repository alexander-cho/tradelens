using ApriumResearch.Mediator;

namespace Tradelens.Social.Application.Posts;

public sealed record CreatePostCommand(
    Guid? ParentPostId,
    IList<string>? Cashtags,
    string Content,
    string? Sentiment
) : ICommand<Guid>;