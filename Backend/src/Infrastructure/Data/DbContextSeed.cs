using System;
using System.Reflection;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class DbContextSeed
{
    public static async Task SeedPostsAsync(TradeLensDbContext context)
    {
        //
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // read data from products.json seed file if DB is empty
        if (!context.Posts.Any())
        {
            // in production
            var postsData = await File.ReadAllTextAsync(path + @"/Data/SeedData/posts.json");
            // var postsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/posts.json");
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
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        if (!context.Stocks.Any())
        {
            var stocksData = await File.ReadAllTextAsync(path + @"/Data/SeedData/tickers.json");
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
