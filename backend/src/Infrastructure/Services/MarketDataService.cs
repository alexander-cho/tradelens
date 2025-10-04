using Core.Interfaces;
using Core.Models;
using Infrastructure.Clients.Finnhub;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public class MarketDataService : IMarketDataService
{
    private readonly IFinnhubClient _finnhubClient;

    public MarketDataService(IFinnhubClient finnhubClient)
    {
        this._finnhubClient = finnhubClient;
    }

    public async Task<MarketStatusModel?> GetMarketStatusAsync()
    {
        var marketStatusDto = await _finnhubClient.GetMarketStatusAsync();
        
        if (marketStatusDto == null)
        {
            throw new InvalidOperationException("Market status data was not available");
        }
        
        var marketStatus = MarketStatusMapper.ToMarketStatusDomainModel(marketStatusDto);

        return marketStatus;
    }
}