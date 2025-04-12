using System.Net.Http.Json;
using Core.DTOs.Tradier;
using Core.Interfaces;
using Infrastructure.Services;
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

    public async Task<OptionsData> GetOptionChainsAsync()
    {
        var client = _httpClientFactory.CreateClient("Tradier");
        
        var response = await client.GetAsync("options/chains?symbol=SOFI&expiration=2025-04-11&greeks=true");
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode) ;
        
        var optionData = await response.Content.ReadFromJsonAsync<OptionsData>();
        
        _logger.LogInformation("Retrieved {optionData}", optionData);

        if (optionData != null)
        {
            return optionData;
        }
        
        throw new HttpRequestException($"Failed to get bar aggregates. Status code: {response.StatusCode}");
    }
}