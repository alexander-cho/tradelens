using Core.Interfaces;

namespace Infrastructure.Services;

public class PolygonService : IPolygonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _polygonApiKey = "UQE0AfiLy65mtQuTOX9mpQZqWV_Qb8iP";

    public PolygonService(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
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