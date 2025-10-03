using Infrastructure.Clients.Finnhub;
using Infrastructure.Clients.Fmp;
using Infrastructure.Clients.Polygon;
using Infrastructure.Clients.Tradier;

namespace API.Extensions;

public static class RegisterDataFetchingExtensions
{
    public static IServiceCollection AddDataClients(this IServiceCollection services)
    {
        services
            .AddScoped<IPolygonClient, PolygonClient>()
            .AddScoped<IFmpClient, FmpClient>()
            .AddScoped<IFinnhubClient, FinnhubClient>()
            .AddScoped<ITradierClient, TradierClient>()
            ;
        
        return services;
    }
}