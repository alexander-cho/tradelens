using Core.Models;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Clients.Fmp;

public interface IFmpClient
{
    Task<IEnumerable<CongressTradesDto>> GetLatestHouseTradesAsync();
    Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync();
    Task<IEnumerable<RevenueSegmentationDto>> GetRevenueProductSegmentationAsync();
    Task<IEnumerable<FinancialMetric>> GetIncomeStatementAsync();
}