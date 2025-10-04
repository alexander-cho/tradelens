using Core.Specifications;
using Infrastructure.Clients.Tradier.DTOs;

namespace Infrastructure.Clients.Tradier;

public interface ITradierClient
{
    Task<OptionsChainDto> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    Task<ExpirationsDto> GetExpiryDatesForUnderlyingAsync(string symbol);
}