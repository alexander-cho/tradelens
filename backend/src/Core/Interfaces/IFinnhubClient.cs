using Core.DTOs.Finnhub;

namespace Core.Interfaces;

public interface IFinnhubClient
{
    Task<MarketStatusDto> GetMarketStatusAsync();
}