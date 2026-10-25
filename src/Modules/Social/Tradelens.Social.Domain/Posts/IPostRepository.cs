namespace Tradelens.Social.Domain.Posts;

public interface IPostRepository
{
    Task Create(Post? parentPost, ICollection<string>? cashtags, string content, Sentiment? sentiment);
    // Task Delete();
}