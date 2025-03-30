using System.Net.Http.Json;
using Core.DTOs.Finnhub;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients;

public class FinnhubClient : IFinnhubClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _finnhubApiKey;
    
    public FinnhubClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._finnhubApiKey = configuration["DataApiKeys:FinnhubApiKey"];
    }

    public async Task<MarketStatusDto> GetMarketStatusAsync()
    {
        var client = this._httpClientFactory.CreateClient("Finnhub");
        
        var response = await client.GetAsync($"market-status?exchange=US&token={_finnhubApiKey}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<MarketStatusDto>();
    }
}