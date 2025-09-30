using Core.Interfaces;
using Infrastructure.Services;

namespace API.Extensions;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services
            .AddScoped<IPolygonService, PolygonService>() // -> BarAggregateService
            .AddScoped<IFinnhubService, FinnhubService>()
            .AddScoped<ITradierService, TradierService>()
            .AddScoped<IMaxPainService, MaxPainService>()
            ;

        return services;
    }
}