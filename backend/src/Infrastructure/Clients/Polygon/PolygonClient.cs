using System.Net.Http.Json;
using Core.Specifications;
using Infrastructure.Clients.Polygon.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Clients.Polygon;

public class PolygonClient : IPolygonClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _polygonApiKey;
    private readonly ILogger<PolygonClient> _logger;

    public PolygonClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
        ILogger<PolygonClient> logger)
    {
        this._httpClientFactory = httpClientFactory;
        this._polygonApiKey = configuration["DataApiKeys:PolygonApiKey"];
        this._logger = logger;
    }

    public async Task<BarAggregateDto?> GetBarAggregatesAsync(PolygonBarAggSpecParams polygonBarAggSpecParams)
    {
        var client = this._httpClientFactory.CreateClient("Polygon");
        var response = await client.GetAsync(
            $"v2/aggs/ticker/{polygonBarAggSpecParams.Ticker.ToUpper()}/range/{polygonBarAggSpecParams.Multiplier}/{polygonBarAggSpecParams.Timespan}/{polygonBarAggSpecParams.From}/{polygonBarAggSpecParams.To}?adjusted=true&sort=asc&limit=50000&apiKey={_polygonApiKey}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<BarAggregateDto>();
        _logger.LogInformation(
            "Retrieved {polygonBarAggSpecParams.Multiplier} {polygonBarAggSpecParams.Timespan} bar aggregates for symbol {polygonBarAggSpecParams.Ticker} from {polygonBarAggSpecParams.From} to {polygonBarAggSpecParams.To}",
            polygonBarAggSpecParams.Multiplier, polygonBarAggSpecParams.Timespan, polygonBarAggSpecParams.Ticker,
            polygonBarAggSpecParams.From, polygonBarAggSpecParams.To);

        return result;
    }

    public async Task<RelatedCompaniesDto?> GetRelatedCompaniesAsync(string ticker)
    {
        var client = this._httpClientFactory.CreateClient("Polygon");
        var response =
            await client.GetAsync(
                $"v1/related-companies/{ticker.ToUpper()}?apiKey={_polygonApiKey}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RelatedCompaniesDto>();
        return result;
    }
}