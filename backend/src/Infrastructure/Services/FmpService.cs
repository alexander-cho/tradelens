using Core.DTOs.Fmp;
using Core.Interfaces;

namespace Infrastructure.Services;

public class FmpService : IFmpService
{
    private readonly IFmpClient _fmpClient;

    public FmpService(IFmpClient fmpClient)
    {
        _fmpClient = fmpClient;
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestHouseTradesAsync()
    {
        return await _fmpClient.GetLatestHouseTradesAsync();
    }

    public async Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync()
    {
        return await _fmpClient.GetLatestSenateTradesAsync();
    }

    public async Task<IEnumerable<RevenueSegmentation>> GetRevenueProductSegmentationAsync()
    {
        return await _fmpClient.GetRevenueProductSegmentationAsync();
    }
}