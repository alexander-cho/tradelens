using Microsoft.EntityFrameworkCore;

using TradeLensAPI.Models;

namespace TradeLensAPI.DbContexts
{
    public class ApiDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ApiDbContext(DbContextOptions<ApiDbContext> options, IConfiguration configuration)
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

        public DbSet<Post> Posts { get; set; }
    }
}
