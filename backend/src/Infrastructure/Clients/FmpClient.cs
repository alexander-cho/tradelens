using System.Net.Http.Json;
using Core.DTOs.Fmp;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients;

public class FmpClient : IFmpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _fmpApiKey;
    
    public FmpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._fmpApiKey = configuration["DataApiKeys:FmpApiKey"];
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestHouseTradesAsync()
    {
        var client = _httpClientFactory.CreateClient("Fmp");
        
        var response = await client.GetAsync($"house-latest?apikey={_fmpApiKey}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();
    }
    
    public async Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync()
    {
        var client = _httpClientFactory.CreateClient("Fmp");
        
        var response = await client.GetAsync($"senate-latest?apikey={_fmpApiKey}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();
    }

    public async Task<IEnumerable<RevenueSegmentation>> GetRevenueProductSegmentationAsync()
    {
        var client = _httpClientFactory.CreateClient("Fmp");
        
        var response = await client.GetAsync($"revenue-product-segmentation?symbol=SOFI&limit=5&period=annual&apikey={_fmpApiKey}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<RevenueSegmentation>>();
    }
}