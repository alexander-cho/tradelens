namespace Tradelens.Api.Extensions;

public static class HttpClientFactoryExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Polygon", client =>
        {
            client.BaseAddress = new Uri("https://api.massive.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        services.AddHttpClient("Fmp", client =>
        {
            client.BaseAddress = new Uri("https://financialmodelingprep.com/stable/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("Finnhub", client =>
        {
            client.BaseAddress = new Uri("https://finnhub.io/api/v1/stock/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        services.AddHttpClient("Tradier", client =>
        {
            client.BaseAddress = new Uri("https://api.tradier.com/v1/markets/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["DataApiKeys:TradierApiKey"]}");
        });

        services.AddHttpClient("Fred", client =>
        {
            client.BaseAddress = new Uri("https://api.stlouisfed.org/fred/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        
        return services;
    }
}