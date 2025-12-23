using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Polly.Retry;

namespace API.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TradelensDbContext>();
        
            // retry policy for Npgsql exceptions
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<NpgsqlException>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount) =>
                    {
                        Console.WriteLine($"Retry {retryCount} due to {exception.Message}");
                    });
        
            await retryPolicy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync();
            });
        
            await DbContextSeed.SeedPostsAsync(context);
            await DbContextSeed.SeedStocksAsync(context);
            await DbContextSeed.SeedCompanyMetricsAsync(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}