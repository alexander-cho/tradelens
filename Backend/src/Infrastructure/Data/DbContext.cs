using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TradeLensDbContext(DbContextOptions options) : DbContext(options)
{
    // define entities
    public required DbSet<Post> Posts { get; set; }
    public required DbSet<Stock> Stocks { get; set; }
}
