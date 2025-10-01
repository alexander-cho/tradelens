using Core.DTOs.Tradier;
using Core.Interfaces;

namespace Infrastructure.Services;

public class OptionsService : IOptionsService
{
    private readonly ITradierClient _client;

    public OptionsService(ITradierClient client)
    {
        _client = client;
    }

    public async Task<ExpiryData> GetExpiryListForUnderlyingAsync(string symbol)
    {
        return await _client.GetExpiryDatesForUnderlyingAsync(symbol);
    }
}