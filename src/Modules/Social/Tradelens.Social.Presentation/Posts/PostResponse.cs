namespace Tradelens.Social.Presentation.Posts;

public record PostResponse(
    Guid Id,
    Guid UserId,
    string Cashtags,
    string Content,
    string? Sentiment,
    DateTime CreatedAt
);