using Tradelens.Infrastructure.Clients.Finnhub;
using Tradelens.Infrastructure.Clients.Fmp;
using Tradelens.Infrastructure.Clients.Fred;
using Tradelens.Infrastructure.Clients.Polygon;
using Tradelens.Infrastructure.Clients.Tradier;

namespace Tradelens.Api.Extensions;

public static class RegisterDataFetchingExtensions
{
    public static IServiceCollection AddDataClients(this IServiceCollection services)
    {
        services
            .AddScoped<IPolygonClient, PolygonClient>()
            .AddScoped<IFmpClient, FmpClient>()
            .AddScoped<IFinnhubClient, FinnhubClient>()
            .AddScoped<ITradierClient, TradierClient>()
            .AddScoped<IFredClient, FredClient>()
            ;
        
        return services;
    }
}