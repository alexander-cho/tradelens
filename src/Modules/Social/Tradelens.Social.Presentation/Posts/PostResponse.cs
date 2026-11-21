namespace Tradelens.Social.Presentation.Posts;

public sealed record PostResponse(
    Guid Id,
    Guid UserId,
    string Cashtags,
    string Content,
    string? Sentiment,
    DateTime CreatedAt
);