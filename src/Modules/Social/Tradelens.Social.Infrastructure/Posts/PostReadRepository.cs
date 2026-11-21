using System.Data;
using Dapper;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Infrastructure.Posts;

public sealed class PostReadRepository : IPostReadRepository
{
    private readonly IDbConnection _db;

    public PostReadRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<PostDto> GetPostByIdAsync(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        // possible null reference return, look into result pattern
        const string sql =
            """SELECT "Id", "UserId", "Cashtags", "Content", "Sentiment_Value" AS "Sentiment", "CreatedAt" FROM "Posts" WHERE "Id" = @id""";
        return await _db.QueryFirstOrDefaultAsync<PostDto>(sql, new { id = query.PostId });
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync(GetPostsQuery query, CancellationToken cancellationToken)
    {
        const string sql =
            """SELECT "Id", "UserId", "Cashtags", "Content", "Sentiment_Value" AS "Sentiment", "CreatedAt" FROM "Posts" ORDER BY "CreatedAt" LIMIT @pageSize""";
        return await _db.QueryAsync<PostDto>(sql, new { pageSize = query.PageSize });
    }
}