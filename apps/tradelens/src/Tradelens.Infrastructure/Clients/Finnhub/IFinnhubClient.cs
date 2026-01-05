using Tradelens.Infrastructure.Clients.Finnhub.DTOs;

namespace Tradelens.Infrastructure.Clients.Finnhub;

public interface IFinnhubClient
{
    Task<MarketStatusDto?> GetMarketStatusAsync();
    Task<FinnhubCompanyProfileDto?> GetCompanyProfileAsync(string ticker);
    Task GetCongressionalTradesByTickerAsync();
}