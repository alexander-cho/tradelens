using ApriumResearch.SharedKernel;
using ApriumResearch.TidyResult;

namespace Tradelens.Social.Domain.Posts;

public sealed class Post : Entity, IAggregateRoot
{
    private const int MaxCashtagsPerPost = 5;
    private const int MinContentLength = 50;
    private const int MinutesToDelete = 5;

    public PostId Id { get; init; }

    // change to strongly typed User id or reference User BC
    public Guid UserId { get; init; }

    // if this Post is a reply to another one, reference to the Parent post
    // think about if we need to reference the whole Post object or just PostId
    public PostId? ParentPostId { get; private set; }

    // need to be a reference to the Financial Instruments aggregate in Fundamentals BC
    public IList<Cashtag>? Cashtags { get; private set; }

    public string Content { get; private set; }
    public Sentiment? Sentiment { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; init; }
    // more properties as needed

    private Post()
    {
    }

    private Post(PostId? parentPostId, IList<Cashtag>? cashtags, string content, Sentiment? sentiment)
    {
        Id = PostId.New();
        UserId = Guid.CreateVersion7();
        ParentPostId = parentPostId;
        Cashtags = cashtags;
        Content = content;
        Sentiment = sentiment;
        Status = new Status("Published");
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new PostCreatedEvent());
    }

    public static Result<Post> Create(PostId? parentPostId, IList<Cashtag>? cashtags, string content, Sentiment? sentiment)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<Post>.Failure(PostErrors.EmptyContent);
        }

        if (cashtags?.Count > MaxCashtagsPerPost)
        {
            return Result<Post>.Failure(PostErrors.MaxCashtagsPerPostLimitExceeded);
        }

        if (content.Length < MinContentLength)
        {
            return Result<Post>.Failure(PostErrors.MinimumContentLengthNotMet);
        }

        return Result<Post>.Success(new Post(parentPostId, cashtags, content, sentiment));
    }

    public Post Edit()
    {
        return new Post();
    }

    public void Delete()
    {
        if (DateTime.UtcNow - CreatedAt > TimeSpan.FromMinutes(MinutesToDelete))
        {
            // return Result<Guid>.Failure(PostErrors.CannotDeletePastTime);
        }

        Status = new Status("Deleted");
    }
}