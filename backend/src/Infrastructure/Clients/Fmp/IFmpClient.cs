using Core.Models;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Clients.Fmp;

public interface IFmpClient
{
    Task<IEnumerable<CongressTradeDto>> GetLatestHouseTradesAsync();
    Task<IEnumerable<CongressTradeDto>> GetLatestSenateTradesAsync();
    Task<IEnumerable<IncomeStatementDto>> GetIncomeStatementAsync(string symbol, int limit, string period);
    Task<IEnumerable<RevenueSegmentationDto>> GetRevenueProductSegmentationAsync();
}