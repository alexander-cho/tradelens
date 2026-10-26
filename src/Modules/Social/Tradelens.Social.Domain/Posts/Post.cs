using Tradelens.SharedKernel;

namespace Tradelens.Social.Domain.Posts;

public class Post : AggregateRoot
{
    private const int MaxCashtagsPerPost = 5;
    private const int MinContentLength = 50;
    private const int MinutesToDelete = 5;
    public PostId Id { get; init; }
    public Guid UserId { get; init; } // change to strongly typed User id or reference User BC

    public Post?
        ParentPost { get; private set; } // if this Post is a reply to another one, reference to the Parent post

    public ICollection<Cashtag>?
        Cashtags
    {
        get;
        private set;
    } // need to be a reference to the Financial Instruments aggregate in Fundamentals BC

    public string Content { get; private set; }
    public Sentiment? Sentiment { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    // more properties as needed

    private Post()
    {
    }

    private Post(Post? parentPost, ICollection<Cashtag>? cashtags, string content, Sentiment? sentiment)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new DomainException("Post content cannot be empty");
        }

        if (cashtags?.Count > MaxCashtagsPerPost)
        {
            throw new DomainException(
                $"Only up to {MaxCashtagsPerPost} cashtags allowed per post for user on <base> tier");
        }

        if (content.Length > MinContentLength)
        {
            throw new DomainException(
                $"Post content length must have a minimum of {MinContentLength} characters");
        }

        Id = new PostId(Guid.CreateVersion7());
        UserId = Guid.CreateVersion7();
        ParentPost = parentPost;
        Cashtags = cashtags;
        Content = content;
        Sentiment = sentiment;
        Status = Status.Published;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new PostCreatedEvent(Id, UserId, CreatedAt));
    }

    public static Post Create(Post? parentPost, ICollection<Cashtag>? cashtags, string content, Sentiment? sentiment)
    {
        return new Post(parentPost, cashtags, content, sentiment);
    }

    public Post Edit()
    {
        return new Post();
    }

    public void Delete()
    {
        if (DateTime.UtcNow - CreatedAt > TimeSpan.FromMinutes(MinutesToDelete))
        {
            throw new DomainException($"Cannot delete a post written more than {MinutesToDelete} minutes ago");
        }

        Status = Status.Deleted;
    }
}