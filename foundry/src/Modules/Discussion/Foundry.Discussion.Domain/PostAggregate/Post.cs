namespace Foundry.Discussion.Domain.PostAggregate;

public class Post // inherit base Entity class, will define in Shared kernel
{
    public int Id { get; private set; } // think of putting in base Entity
    public int UserId { get; private set; } // User is going to be in a different bounded context all together
    public string Text { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private Post()
    {
    }

    private Post(int userId, string text)
    {
        UserId = userId;
        Text = text;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Post Create(string text, int userId)
    {
        // var post = new Post()
        // {
        //     UserId = userId,
        //     Text = text,
        //     CreatedAtUtc = DateTime.UtcNow
        // };
        //
        // // raise a domain event?
        //
        // return post;
        return new Post(userId, text);
    }
}