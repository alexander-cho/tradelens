using System.Net.Http.Json;
using Infrastructure.Clients.Finnhub.DTOs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients.Finnhub;

public class FinnhubClient : IFinnhubClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _finnhubApiKey;

    public FinnhubClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._finnhubApiKey = configuration["DataApiKeys:FinnhubApiKey"];
    }

    public async Task<MarketStatusDto?> GetMarketStatusAsync()
    {
        try
        {
            var client = this._httpClientFactory.CreateClient("Finnhub");

            var response = await client.GetAsync($"market-status?exchange=US&token={_finnhubApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MarketStatusDto>();

            return result;
        }
        catch(HttpRequestException exception)
        {
            throw new HttpRequestException("Failed to fetch current market status", exception);
        }
    }

    // PRO endpoint
    public Task GetCongressionalTradesByTickerAsync()
    {
        throw new NotImplementedException();
    }
}