using System.Net.Http.Json;
using Infrastructure.Clients.Fred.DTOs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Clients.Fred;

public class FredClient : IFredClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _fredApiKey;

    public FredClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this._httpClientFactory = httpClientFactory;
        this._fredApiKey = configuration["DataApiKeys:FredApiKey"];
    }

    public async Task<MarginBalanceDto?> GetMarginBalanceAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fred");
            
            var response = await client.GetAsync($"series/observations?series_id=BOGZ1FL663067003Q&api_key={_fredApiKey}&file_type=json");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MarginBalanceDto>();

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch Security Brokers and Dealers; Receivables Due from Customers (Margin Loans and Other Receivables); Asset, Level", ex);
        }
    }

    public async Task<MoneyMarketFundsDto?> GetMoneyMarketFundsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Fred");
            
            var response = await client.GetAsync($"series/observations?series_id=MMMFFAQ027S&api_key={_fredApiKey}&file_type=json");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MoneyMarketFundsDto>();

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to fetch Money Market Funds; Total Financial Assets, Level", ex);
        }
    }
}