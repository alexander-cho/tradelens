namespace Tradelens.Social.Domain.Posts;

public readonly record struct PostId(Guid Value)
{
    public static PostId New()
    {
        return new PostId(Guid.CreateVersion7());
    }
};