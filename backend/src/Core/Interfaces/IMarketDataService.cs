using Core.Models;

namespace Core.Interfaces;

public interface IMarketDataService
{
    Task<MarketStatusModel?> GetMarketStatusAsync();
}