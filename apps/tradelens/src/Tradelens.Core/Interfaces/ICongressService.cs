using Tradelens.Core.Models;

namespace Tradelens.Core.Interfaces;

public interface ICongressService
{
    Task<IEnumerable<CongressTradeModel>> GetCongressTradesAsync(string chamber);
}