using Core.DTOs.Tradier;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class MaxPainService : IMaxPainService
{
    private readonly ITradierClient _client;

    public MaxPainService(ITradierClient client)
    {
        _client = client;
    }
    public async Task<OptionsData> GetMaxPainCalculation(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        return await _client.GetOptionChainsAsync(tradierOptionChainSpecParams);
    }
}