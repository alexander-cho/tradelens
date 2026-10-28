using Microsoft.EntityFrameworkCore;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Infrastructure;

public class SocialDbContext : DbContext
{
    public required DbSet<Post> Posts { get; set; }
}