using System.Net.Http.Json;
using Core.DTOs.Fmp;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients;

public class FmpClient : IFmpClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _fmpApiKey;
    private const string BaseUrl = "https://financialmodelingprep.com/stable/";
    
    public FmpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("Polygon");
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _fmpApiKey = configuration["DataApiKeys:FmpApiKey"];
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestHouseTradesAsync()
    {
        var url = $"house-latest?apikey={_fmpApiKey}";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync()
    {
        var url = $"senate-latest?apikey={_fmpApiKey}";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();
    }
}