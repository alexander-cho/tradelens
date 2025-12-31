using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TradelensDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    // define entities
    public required DbSet<Post> Posts { get; set; }
    public required DbSet<Stock> Stocks { get; set; }
    public required DbSet<CompanyMetric> CompanyMetrics { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<CompanyMetric>(entity =>
        {
            entity.HasOne(cm => cm.Stock)
                .WithMany()
                .HasForeignKey(cm => cm.Ticker)
                .HasPrincipalKey(s => s.Ticker);
            
            entity.HasIndex(cm => new { cm.Ticker, cm.Period, cm.Year, cm.Metric });
            entity.HasIndex(cm => new { cm.Ticker, cm.Year });
            entity.HasIndex(cm => cm.Metric);
        });
    }
}
