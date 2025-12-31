using Infrastructure.Clients.Finnhub.DTOs;

namespace Infrastructure.Clients.Finnhub;

public interface IFinnhubClient
{
    Task<MarketStatusDto?> GetMarketStatusAsync();
    Task<FinnhubCompanyProfileDto?> GetCompanyProfileAsync(string ticker);
    Task GetCongressionalTradesByTickerAsync();
}