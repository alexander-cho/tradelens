using System.Net.Http.Json;
using Infrastructure.Clients.Fmp.DTOs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients.Fmp;

public class FmpClient : IFmpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _fmpApiKey;

    public FmpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._fmpApiKey = configuration["DataApiKeys:FmpApiKey"];
    }

    public async Task<IEnumerable<CongressTradeDto>> GetLatestHouseTradesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");
            
            var response = await client.GetAsync($"house-latest?apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradeDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch house trades", ex);
        }
    }

    public async Task<IEnumerable<CongressTradeDto>> GetLatestSenateTradesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");

            var response = await client.GetAsync($"senate-latest?apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradeDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch senate trades", ex);
        }
    }
    
    public async Task<IEnumerable<IncomeStatementDto>> GetIncomeStatementAsync(string symbol, int limit, string period)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");

            var response = await client.GetAsync($"income-statement?symbol={symbol}&limit=5&period={period}&apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<IncomeStatementDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch income statement", ex);
        }
    }

    public async Task<IEnumerable<RevenueSegmentationDto>> GetRevenueProductSegmentationAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");

            var response = await client.GetAsync($"revenue-product-segmentation?symbol=SOFI&limit=5&period=annual&apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<RevenueSegmentationDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch revenue product segmentation", ex);
        }
    }
}