using Tradelens.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tradelens.Api.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddPostgresqlDbContext(this IServiceCollection services, IConfiguration config)
    { 
        services.AddDbContext<TradelensDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}