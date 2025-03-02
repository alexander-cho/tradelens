using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace API.Tests;

internal class TradelensWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("Tradelens")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // configure to use test database instead of actual db
            services.RemoveAll(typeof(DbContextOptions<TradeLensDbContext>));
            
            // get test db connection string defined in user secrets
            // var configuration = new ConfigurationBuilder().AddUserSecrets<TradelensWebApplicationFactory>().Build();
            // var connectionString = configuration.GetConnectionString("TradeLensTestDB");
            // services.AddSqlServer<TradeLensDbContext>(connectionString);
            // services.AddSqlServer<TradeLensDbContext>("Server=localhost,2500;Database=TradeLensTest;User ID=SA;Password=Password@1;TrustServerCertificate=True");

            services.AddDbContext<TradeLensDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
            
            // make sure db context is disposed of and newly recreated for each test run
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TradeLensDbContext>();
            dbContext.Database.EnsureDeleted();
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}