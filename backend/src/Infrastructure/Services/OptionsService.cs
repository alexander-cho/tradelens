using Core.Interfaces;
using Infrastructure.Clients.Tradier;

namespace Infrastructure.Services;

public class OptionsService : IOptionsService
{
    private readonly ITradierClient _client;

    public OptionsService(ITradierClient client)
    {
        _client = client;
    }

    public async Task<string> GetExpiryListForUnderlyingAsync(string symbol)
    {
        // return await _client.GetExpiryDatesForUnderlyingAsync(symbol);

        throw new NotImplementedException();
    }
}