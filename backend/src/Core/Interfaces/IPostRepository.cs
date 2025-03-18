using Core.Entities;

namespace Core.Interfaces;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> GetPostsAsync(string? ticker, string? sentiment, string? sort);

    // give optional, so handle nullable return case in the controller
    Task<Post?> GetPostByIdAsync(int id);

    // not Task<> since not database operations at this point, just changing entity state in memory
    void AddPost(Post post);
    void UpdatePost(Post post);
    void DeletePost(Post post);
    bool PostExists(int id);
    Task<bool> SaveChangesAsync();
    
    Task<IReadOnlyList<string>> GetTickers();
}
