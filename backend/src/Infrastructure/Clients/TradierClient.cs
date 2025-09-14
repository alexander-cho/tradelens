using System.Net.Http.Json;
using Core.DTOs.Tradier;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Clients;

public class TradierClient : ITradierClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TradierClient> _logger;
    
    public TradierClient(IHttpClientFactory httpClientFactory, ILogger<TradierClient> logger)
    {
        this._httpClientFactory = httpClientFactory;
        this._logger = logger;
    }

    public async Task<OptionsData> GetOptionChainsAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        var client = _httpClientFactory.CreateClient("Tradier");
        
        var response = await client.GetAsync($"options/chains?symbol={tradierOptionChainSpecParams.Symbol}&expiration={tradierOptionChainSpecParams.Expiration}&greeks={tradierOptionChainSpecParams.Greeks}");
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var optionData = await response.Content.ReadFromJsonAsync<OptionsData>();

            _logger.LogInformation("Retrieved {optionData}", optionData);

            if (optionData != null)
            {
                return optionData;
            }
        }

        throw new HttpRequestException($"Failed to get bar aggregates. Status code: {response.StatusCode}");
    }

    public async Task<ExpiryData> GetExpiryDatesForUnderlyingAsync(string symbol)
    {
        var client = _httpClientFactory.CreateClient("Tradier");
        
        var response = await client.GetAsync($"options/expirations?symbol={symbol}");
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var expiryDates = await response.Content.ReadFromJsonAsync<ExpiryData>();

            _logger.LogInformation("Retrieved {expiryData}", expiryDates);

            if (expiryDates != null)
            {
                return expiryDates;
            }
        }
        
        throw new HttpRequestException($"Failed to get expiry dates for {symbol}");
    }
}