using Tradelens.Core.Models;

namespace Tradelens.Core.Interfaces;

public interface IMarketDataService
{
    Task<MarketStatusModel?> GetMarketStatusAsync();
    Task<IEnumerable<EarningsCalendarModel>> GetEarningsCalendarAsync(string from, string to);
}