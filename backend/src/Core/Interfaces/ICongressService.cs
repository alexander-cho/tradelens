using Core.Models;

namespace Core.Interfaces;

public interface ICongressService
{
    Task<IEnumerable<CongressTradeModel>> GetCongressTradesAsync(string chamber);
}