using System.Net.Http.Json;
using Core.DTOs.Tradier;
using Core.Interfaces;

namespace Infrastructure.Clients;

public class TradierClient : ITradierClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public TradierClient(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public async Task<OptionsData> GetOptionChainsAsync()
    {
        var client = _httpClientFactory.CreateClient("Tradier");
        
        var response = await client.GetAsync("options/chains?symbol=SOFI&expiration=2025-04-11&greeks=true");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<OptionsData>();
    }
}