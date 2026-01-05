using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Tradelens.Infrastructure.Clients.Fred.DTOs;

namespace Tradelens.Infrastructure.Clients.Fred;

public class FredClient : IFredClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _fredApiKey;

    public FredClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._fredApiKey = configuration["DataApiKeys:FredApiKey"];
    }

    public async Task<SeriesDto?> GetSeriesAsync(string seriesId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fred");
            
            var response = await client.GetAsync($"series?series_id={seriesId}&api_key={_fredApiKey}&file_type=json");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<SeriesDto>();

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to get series data for series Id {seriesId}", ex);
        }
    }
    
    public async Task<SeriesObservationsDto?> GetSeriesObservationsAsync(string seriesId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fred");
            
            var response = await client.GetAsync($"series/observations?series_id={seriesId}&api_key={_fredApiKey}&file_type=json");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<SeriesObservationsDto>();

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to get series observations data for series Id {seriesId}", ex);
        }
    }
}