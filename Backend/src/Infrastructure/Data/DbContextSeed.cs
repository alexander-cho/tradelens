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

    public static async Task SeedCompanyData(TradeLensDbContext context)
    {
        if (!context.Stocks.Any())
        {
            var stocksData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/tickers.json");
            var stocks = JsonSerializer.Deserialize<List<Stock>>(stocksData);

            // if there is no data
            if (stocks == null)
            {
                return;
            }

            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();
        }
    }
}
