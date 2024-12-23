using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class DbContextSeed
{
    public static async Task SeedPostsAsync(TradeLensDbContext context)
    {
        // read data from products.json seed file if DB is empty
        if (!context.Posts.Any())
        {
            var postsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/posts.json");
            var posts = JsonSerializer.Deserialize<List<Post>>(postsData);

            // if there is no data
            if (posts == null)
            {
                return;
            }

            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();
        }
    }

    // public static async Task SeedCompanyData(StoreContext context)
    // {

    // }
}
