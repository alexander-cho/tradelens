using Tradelens.Core.Interfaces;
using Tradelens.Core.Models;
using Tradelens.Infrastructure.Clients.Finnhub;
using Tradelens.Infrastructure.Clients.Fmp;
using Tradelens.Infrastructure.Mappers;

namespace Tradelens.Infrastructure.Services;

public class MarketDataService : IMarketDataService
{
    private readonly IFinnhubClient _finnhubClient;
    private readonly IFmpClient _fmpClient;

    public MarketDataService(IFinnhubClient finnhubClient, IFmpClient fmpClient)
    {
        this._finnhubClient = finnhubClient;
        this._fmpClient = fmpClient;
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

    public async Task<IEnumerable<EarningsCalendarModel>> GetEarningsCalendarAsync(string from, string to)
    {
        var earningsCalendarDto = await _fmpClient.GetEarningsCalendarAsync(from, to);

        if (earningsCalendarDto == null)
        {
            throw new InvalidOperationException("Market status data was not available");
        }

        var earningsCalendar =
            earningsCalendarDto.Select(EarningsCalendarMapper.ToEarningsCalendarDomainModel).ToList();

        return earningsCalendar;
    }
}