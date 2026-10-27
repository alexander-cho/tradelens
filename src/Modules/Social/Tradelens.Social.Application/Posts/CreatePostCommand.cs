using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Application.Posts;

public sealed record CreatePostCommand(
    Post? ParentPost,
    ICollection<Cashtag>? Cashtags,
    string Content,
    Sentiment Sentiment
) : ICommand<PostId>;