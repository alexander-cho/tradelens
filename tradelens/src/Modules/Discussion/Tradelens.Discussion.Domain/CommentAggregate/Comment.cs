namespace Tradelens.Discussion.Domain.CommentAggregate;

public class Comment
{
    public int Id { get; private set; }
    public int PostId { get; private set; }
    public int UserId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    
    private Comment()
    {
    }

    // private Comment()
    // {
    // }

    // public static Comment Create(string text, int postId)
    // {
    //     
    // }
}