using Core.Models;
using Core.Specifications;

namespace Core.Interfaces;

public interface IOptionsService
{
    public Task<OptionsChainModel> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
    public Task<CallsAndPutsCashSums> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    public Task<ExpirationsModel> GetExpiryListForUnderlyingAsync(string symbol);
}