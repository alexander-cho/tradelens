using Core.DTOs.Tradier;
using Core.Specifications;

namespace Core.Interfaces;

public interface ITradierClient
{
    Task<OptionsData> GetOptionChainsAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    Task<ExpiryData> GetExpiryDatesForUnderlyingAsync(string symbol);
}