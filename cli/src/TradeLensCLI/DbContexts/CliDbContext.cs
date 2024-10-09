using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using TradeLensCLI.Models;

namespace TradeLensCLI.DbContexts
{
    public class CliDbContext : DbContext
    {
        public string DbPath { get; }
        public DbSet<Post> Posts { get; set; }

        public CliDbContext()
        {
            DbPath = "TradeLensCLI.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
