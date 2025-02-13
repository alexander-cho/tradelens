using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class PolygonService : IPolygonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _polygonApiKey;

    public PolygonService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._polygonApiKey = configuration["DataApiKeys:PolygonApiKey"];
    }

    // PASS PARAMS LATER
    public async Task<string> GetBarAggregatesAsync()
    {
        var client = this._httpClientFactory.CreateClient("Polygon");
        var response =
            await client.GetAsync(
                $"v2/aggs/ticker/SOFI/range/1/hour/2025-01-01/2025-01-17?adjusted=true&sort=asc&limit=50000&apiKey={_polygonApiKey}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        return "Could not get Bar Aggregates";
    }

    public async Task<string> GetRelatedCompaniesAsync()
    {
        var client = this._httpClientFactory.CreateClient("Polygon");
        var response =
            await client.GetAsync(
                $"v1/related-companies/SOFI?apiKey={_polygonApiKey}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        return "Could not get related companies";
    }
}