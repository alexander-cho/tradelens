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
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");
            
            var response = await client.GetAsync($"house-latest?apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch house trades", ex);
        }
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");

            var response = await client.GetAsync($"senate-latest?apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CongressTradesDto>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch senate trades", ex);
        }
    }

    public async Task<IEnumerable<RevenueSegmentation>> GetRevenueProductSegmentationAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fmp");

            var response = await client.GetAsync($"revenue-product-segmentation?symbol=SOFI&limit=5&period=annual&apikey={_fmpApiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<RevenueSegmentation>>();

            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch revenue product segmentation", ex);
        }
    }

    public Task<IEnumerable<FinancialMetric>> GetIncomeStatementAsync()
    {
        throw new NotImplementedException();
    }
}