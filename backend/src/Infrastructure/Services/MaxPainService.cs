using Core.DTOs.Tradier;
using Core.Interfaces;

namespace Infrastructure.Services;

public class MaxPainService : IMaxPainService
{
    private readonly ITradierClient _client;

    public MaxPainService(ITradierClient client)
    {
        _client = client;
    }
    public async Task<OptionsData> GetOptionChainsAsync()
    {
        return await _client.GetOptionChainsAsync();
    }
}