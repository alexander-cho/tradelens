using System.Net.Http.Json;
using Core.DTOs.Finnhub;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients;

public class FinnhubClient : IFinnhubClient
{
    private readonly HttpClient _httpClient;
    private readonly string? _finnhubApiKey;
    private const string BaseUrl = "https://finnhub.io/api/v1/stock/";
    
    public FinnhubClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("Finnhub");
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _finnhubApiKey = configuration["DataApiKeys:FinnhubApiKey"];
    }

    public async Task<MarketStatusDto> GetMarketStatusAsync()
    {
        var url = $"market-status?exchange=US&token={_finnhubApiKey}";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<MarketStatusDto>();
    }
}