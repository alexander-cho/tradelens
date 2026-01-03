using Tradelens.Core.Models;
using Tradelens.Core.Specifications;

namespace Tradelens.Core.Interfaces;

public interface IOptionsService
{
    public Task<OptionsChainModel> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
    public Task<CallsAndPutsCashSums> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    public Task<ExpirationsModel> GetExpiryListForUnderlyingAsync(string symbol);
}