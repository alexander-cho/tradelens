using Microsoft.EntityFrameworkCore;

using TradeLensApi.Models;

namespace TradeLensApi.DbContexts;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings, with db password stored in secrets
        var dbPassword = Configuration["ConnectionStrings:DefaultConnection:POSTGRESPASSWORD"];
        var baseConnectionString = Configuration.GetConnectionString("DefaultConnection");
        var connectionStringWithPassword = $"{baseConnectionString}; Password={dbPassword}";
        options.UseNpgsql(connectionStringWithPassword);
    }

    public DbSet<Post> Posts { get; set; } = null!;
}
