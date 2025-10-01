using Core.DTOs.Fmp;

namespace Core.Interfaces;

public interface ICongressService
{
    Task<IEnumerable<CongressTradesDto>> GetCongressTradesAsync(string chamber);
}