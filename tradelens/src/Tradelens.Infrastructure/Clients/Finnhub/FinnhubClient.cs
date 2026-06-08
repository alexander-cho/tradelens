using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Tradelens.Infrastructure.Clients.Finnhub.DTOs;

namespace Tradelens.Infrastructure.Clients.Finnhub;

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

            var result = await response.Content.ReadFromJsonAsync<MarketStatusDto?>();

            return result;
        }
        catch(HttpRequestException exception)
        {
            throw new HttpRequestException("Failed to fetch current market status", exception);
        }
    }
    
    public async Task<FinnhubCompanyProfileDto?> GetCompanyProfileAsync(string symbol)
    {
        try
        {
            var client = this._httpClientFactory.CreateClient("Finnhub");

            var response = await client.GetAsync($"profile2?symbol={symbol}&token={_finnhubApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<FinnhubCompanyProfileDto?>();

            return result;
        }
        catch(HttpRequestException exception)
        {
            throw new HttpRequestException($"Failed to fetch company profile for {symbol}", exception);
        }
    }

    public async Task<IReadOnlyList<SecFilingDto>?> GetSecFilingsAsync(string symbol)
    {
        try
        {
            var client = this._httpClientFactory.CreateClient("Finnhub");

            var response = await client.GetAsync($"filings?symbol={symbol}&form=10-K&token={_finnhubApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<SecFilingDto>>();

            return result;
        }
        catch(HttpRequestException exception)
        {
            throw new HttpRequestException($"Failed to fetch SEC filings list for {symbol}", exception);
        }
    }

    // PRO endpoint
    public Task GetCongressionalTradesByTickerAsync()
    {
        throw new NotImplementedException();
    }
}