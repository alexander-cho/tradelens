using Core.DTOs.Fmp;

namespace Core.Interfaces;

public interface IFmpService
{
    Task<IEnumerable<CongressTradesDto>> GetLatestHouseTradesAsync();
    Task<IEnumerable<CongressTradesDto>> GetLatestSenateTradesAsync();
}