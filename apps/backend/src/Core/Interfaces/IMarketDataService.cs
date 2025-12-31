using Core.Models;

namespace Core.Interfaces;

public interface IMarketDataService
{
    Task<MarketStatusModel?> GetMarketStatusAsync();
    Task<IEnumerable<EarningsCalendarModel>> GetEarningsCalendarAsync(string from, string to);
}