using Core.DTOs.Tradier;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces;

public interface ITradierClient
{
    Task<OptionsData> GetOptionChainsAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    Task<ExpiryData> GetExpiryDataForUnderlyingAsync(string symbol);
}