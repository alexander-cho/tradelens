using Microsoft.EntityFrameworkCore;
using Tradelens.Social.Domain.Posts;

namespace Tradelens.Social.Infrastructure;

public sealed class SocialDbContext : DbContext
{
    public SocialDbContext(DbContextOptions<SocialDbContext> options) : base(options)
    {
    }

    public required DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=fluent-api#keys-on-complex-type-properties
        modelBuilder.Entity<Post>().HasKey(p => p.Id);

        modelBuilder.Entity<Post>().Property(p => p.Id)
            .HasConversion(id => id.Value, value => new PostId(value))
            .ValueGeneratedNever();

        modelBuilder.Entity<Post>(builder =>
        {
            builder.ComplexProperty(p => p.Sentiment);
            builder.ComplexProperty(p => p.Status);
        });

        // https://learn.microsoft.com/en-us/ef/core/modeling/complex-types?tabs=data-annotations#collections-of-complex-types
        modelBuilder.Entity<Post>().ComplexCollection(p => p.Cashtags, builder => builder.ToJson());
    }
}