using System.Reflection;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public static class DbContextSeed
{
    public static async Task SeedPostsAsync(TradelensDbContext context)
    {
        // var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // read data from posts.json seed file if DB is empty
        if (!context.Posts.Any())
        {
            // in production
            // var postsData = await File.ReadAllTextAsync(path + @"/Data/SeedData/posts.json");
            
            // in development
            var postsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/posts.json");
            
            // for docker-compose (?)
            // var path = Path.Combine("Infrastructure", "Data", "SeedData", "posts.json");
            // var postsData = await File.ReadAllTextAsync(path);
            
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

    public static async Task SeedCompanyData(TradelensDbContext context)
    {
        // var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        if (!context.Stocks.Any())
        {
            // var stocksData = await File.ReadAllTextAsync(path + @"/Data/SeedData/tickers.json");
            
            var stocksData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/tickers.json");
            
            // var path = Path.Combine("Infrastructure", "Data", "SeedData", "tickers.json");
            // var stocksData = await File.ReadAllTextAsync(path);
            
            var stocks = JsonSerializer.Deserialize<List<Stock>>(stocksData);
    
            if (stocks == null)
            {
                return;
            }
    
            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();
        }
    }
    
    public static async Task SeedCompanyMetricsAsync(TradelensDbContext context)
    {
        if (!context.CompanyMetrics.Any())
        {
            var basePath = "../Infrastructure/Data/SeedData/CompanyFundamentalsSecParse";
        
            if (!Directory.Exists(basePath))
            {
                Console.WriteLine($"Metrics seed directory not found: {basePath}");
                return;
            }
        
            var allMetrics = new List<CompanyMetric>();
        
            // get all ticker directories
            var tickerDirs = Directory.GetDirectories(basePath);
        
            foreach (var tickerDir in tickerDirs)
            {
                // get all JSON files in this ticker's directory
                var jsonFiles = Directory.GetFiles(tickerDir, "*.json");
            
                foreach (var file in jsonFiles)
                {
                    try
                    {
                        var metricsData = await File.ReadAllTextAsync(file);
                        var metrics = JsonSerializer.Deserialize<List<CompanyMetric>>(metricsData);
                    
                        if (metrics != null && metrics.Any())
                        {
                            allMetrics.AddRange(metrics);
                            Console.WriteLine($"Loaded {metrics.Count} metrics from {Path.GetFileName(file)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading {file}: {ex.Message}");
                    }
                }
            }
        
            if (allMetrics.Count > 0)
            {
                context.CompanyMetrics.AddRange(allMetrics);
                await context.SaveChangesAsync();
            }
        }
    }
}
