using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TradeLensDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    // define entities
    public required DbSet<Post> Posts { get; set; }
    public required DbSet<Stock> Stocks { get; set; }
}
