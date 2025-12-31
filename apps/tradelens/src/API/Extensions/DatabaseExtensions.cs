using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

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