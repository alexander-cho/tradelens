using Core.DTOs.Finnhub;

namespace Core.Interfaces;

public interface IMarketDataService
{
    Task<MarketStatusDto?> GetMarketStatusAsync();
}