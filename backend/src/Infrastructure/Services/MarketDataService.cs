using Core.Interfaces;
using Infrastructure.Clients.Finnhub;

namespace Infrastructure.Services;

public class MarketDataService : IMarketDataService
{
    private readonly IFinnhubClient _finnhubClient;

    public MarketDataService(IFinnhubClient finnhubClient)
    {
        this._finnhubClient = finnhubClient;
    }

    public async Task<string> GetMarketStatusAsync()
    {
        // return await _finnhubClient.GetMarketStatusAsync();
        
        throw new NotImplementedException();
    }
}