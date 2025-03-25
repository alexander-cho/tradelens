using Core.DTOs.Finnhub;
using Core.Interfaces;

namespace Infrastructure.Services;

public class FinnhubService : IFinnhubService
{
    private readonly IFinnhubClient _client;

    public FinnhubService(IFinnhubClient client)
    {
        _client = client;
    }

    public async Task<MarketStatusDto> GetMarketStatusAsync()
    {
        return await _client.GetMarketStatusAsync();
    }
}