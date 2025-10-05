using Core.Interfaces;
using Infrastructure.Services;

namespace API.Extensions;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMarketDataService, MarketDataService>()
            .AddScoped<IOptionsService, OptionsService>()
            .AddScoped<ICongressService, CongressService>()
            .AddScoped<ICompanyFundamentalsService, CompanyFundamentalsService>()
            ;

        return services;
    }
}