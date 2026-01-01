using Core.Interfaces;
using Infrastructure.Services;
using StackExchange.Redis;

namespace API.Extensions;

// redis caching service functionality is a singleton so users can access same instance
// Redis, singleton so it's alive for the duration of the application

public static class CachingExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddSingleton<IResponseCacheService, ResponseCacheService>()
            .AddSingleton<IConnectionMultiplexer>(cfg =>
            {
                var connectionString = config.GetConnectionString("Redis");
                if (connectionString == null)
                {
                    throw new Exception("Cannot get Redis connection string");
                }

                var configuration = ConfigurationOptions.Parse(connectionString, ignoreUnknown: true);
                return ConnectionMultiplexer.Connect(configuration);
            });

        return services;
    }
}