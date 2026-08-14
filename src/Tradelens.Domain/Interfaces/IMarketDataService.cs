using Tradelens.Domain.Models;

namespace Tradelens.Domain.Interfaces;

public interface IMarketDataService
{
    Task<MarketStatusModel?> GetMarketStatusAsync();
    Task<IEnumerable<EarningsCalendarModel>> GetEarningsCalendarAsync(string from, string to);
}