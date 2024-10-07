using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using TradeLensCli.Models;

namespace TradeLensCli.DbContexts
{
    public class CliDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public string DbPath { get; }

        public CliDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "sqlite3.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}