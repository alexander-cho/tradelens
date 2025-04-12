using Core.DTOs.Tradier;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class MaxPainService : IMaxPainService
{
    private readonly ITradierClient _client;

    public MaxPainService(ITradierClient client)
    {
        _client = client;
    }
    public async Task<OptionsData> GetMaxPainCalculation()
    {
        return await _client.GetOptionChainsAsync();
    }
}