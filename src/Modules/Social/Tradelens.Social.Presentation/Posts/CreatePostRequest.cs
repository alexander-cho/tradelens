namespace Tradelens.Social.Presentation.Posts;

public sealed record CreatePostRequest(
    Guid? ParentPostId,
    IList<string>? Cashtags,
    string Content,
    string? Sentiment
);