using Tradelens.Domain.Models;
using Tradelens.Domain.Specifications;

namespace Tradelens.Domain.Interfaces;

public interface IOptionsService
{
    public Task<OptionsChainModel> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
    public Task<CallsAndPutsCashSums> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    public Task<ExpirationsModel> GetExpiryListForUnderlyingAsync(string symbol);
}