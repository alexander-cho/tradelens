using Tradelens.Core.Interfaces;
using Tradelens.Infrastructure.Services;

namespace Tradelens.Api.Extensions;

public static class RegisterServiceExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services
            .AddScoped<IMarketDataService, MarketDataService>()
            .AddScoped<IOptionsService, OptionsService>()
            .AddScoped<ICongressService, CongressService>()
            .AddScoped<ICompanyFundamentalsService, CompanyFundamentalsService>()
            .AddScoped<IMacroService, MacroService>()
            ;

        return services;
    }
}