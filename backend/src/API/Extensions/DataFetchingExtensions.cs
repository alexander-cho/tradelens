using Core.Interfaces;
using Infrastructure.Clients;
using Infrastructure.Services;

namespace API.Extensions;

public static class DataFetchingExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services
            // Polygon services
            .AddScoped<IPolygonService, PolygonService>()
            .AddScoped<IPolygonClient, PolygonClient>()
            // FMP services
            .AddScoped<IFmpService, FmpService>()
            .AddScoped<IFmpClient, FmpClient>()
            // Finnhub services
            .AddScoped<IFinnhubService, FinnhubService>()
            .AddScoped<IFinnhubClient, FinnhubClient>()
            // Tradier services
            .AddScoped<ITradierClient, TradierClient>()
            .AddScoped<ITradierService, TradierService>()
            // Option maximum pain
            .AddScoped<IMaxPainService, MaxPainService>();

        return services;
    }
}