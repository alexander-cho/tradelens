using Core.DTOs.Finnhub;
using Core.Interfaces;

namespace Infrastructure.Services;

public class MarketDataService : IMarketDataService
{
    private readonly IFinnhubClient _finnhubClient;

    public MarketDataService(IFinnhubClient finnhubClient)
    {
        this._finnhubClient = finnhubClient;
    }

    public async Task<MarketStatusDto?> GetMarketStatusAsync()
    {
        return await _finnhubClient.GetMarketStatusAsync();
    }
}