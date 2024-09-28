using Microsoft.EntityFrameworkCore;

using TradeLensApi.Models;

namespace TradeLensApi.DbContexts;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = null!;
}
