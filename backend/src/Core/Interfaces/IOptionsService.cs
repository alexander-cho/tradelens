using Core.Models;
using Core.Specifications;

namespace Core.Interfaces;

public interface IOptionsService
{
    CallsAndPutsCashSums CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    public Task<ExpirationsModel> GetExpiryListForUnderlyingAsync(string symbol);
}