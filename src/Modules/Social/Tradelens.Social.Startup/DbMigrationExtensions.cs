using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Infrastructure;

namespace Tradelens.Social.Startup;

public static class DbMigrationExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<SocialDbContext>();
            await context.Database.MigrateAsync();
            await DbContextSeed.SeedPostsAsync(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}