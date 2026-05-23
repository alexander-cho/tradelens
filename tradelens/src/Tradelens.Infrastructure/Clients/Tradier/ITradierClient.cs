using Tradelens.Core.Specifications;
using Tradelens.Infrastructure.Clients.Tradier.DTOs;

namespace Tradelens.Infrastructure.Clients.Tradier;

public interface ITradierClient
{
    Task<OptionsChainDto> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);

    Task<ExpirationsDto> GetExpiryDatesForUnderlyingAsync(string symbol);
}