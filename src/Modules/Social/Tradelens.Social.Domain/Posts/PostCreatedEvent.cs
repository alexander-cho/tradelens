namespace Tradelens.Social.Domain.Posts;

public record PostCreatedEvent(PostId Id, Guid UserId, DateTime CreatedAt);