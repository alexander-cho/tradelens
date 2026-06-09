using Tradelens.Domain.Models;

namespace Tradelens.Domain.Interfaces;

public interface ICongressService
{
    Task<IEnumerable<CongressTradeModel>> GetCongressTradesAsync(string chamber);
}