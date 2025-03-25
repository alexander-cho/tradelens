using Core.DTOs.Finnhub;

namespace Core.Interfaces;

public interface IFinnhubService
{
    Task<MarketStatusDto> GetMarketStatusAsync();
}