using System.Data;
using Dapper;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Infrastructure.Posts;

public class PostReadRepository : IPostReadRepository
{
    private readonly IDbConnection _db;

    public PostReadRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<PostResponse> GetPostByIdAsync(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        // possible null reference return, look into result pattern
        const string sql =
            """SELECT "Id", "UserId", "Cashtags", "Content", "Sentiment_Value" AS "Sentiment", "CreatedAt" FROM "Posts" WHERE "Id" = @id""";
        return await _db.QueryFirstOrDefaultAsync<PostResponse>(sql, new { id = query.PostId });
    }

    public async Task<IEnumerable<PostResponse>> GetPostsAsync(GetPostsQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}