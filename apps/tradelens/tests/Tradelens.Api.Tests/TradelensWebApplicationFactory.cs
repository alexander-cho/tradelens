using Tradelens.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Tradelens.Api.Tests;

public class TradelensWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.2-alpine3.21")
        .WithDatabase("test-tradelens-db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(5555, 5432)
        .Build();
    
    public TradelensDbContext Db { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<TradelensDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TradelensDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }

    // take care of starting and stopping container
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Db = Services.CreateScope().ServiceProvider.GetRequiredService<TradelensDbContext>();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}