namespace API.Extensions;

public static class HttpClientFactoryExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("Polygon", client =>
        {
            client.BaseAddress = new Uri("https://api.polygon.io/");
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
        
        return services;
    }
}