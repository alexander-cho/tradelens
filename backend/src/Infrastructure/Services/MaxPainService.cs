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

    public async Task<OptionsData> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        // this should return a hashmap containing cash values for calls, puts, and the max pain strike.
        // Each strike price is considered as a hypothetical closing price at expiry
        
        var ticker = tradierOptionChainSpecParams.Symbol;

        List<int> putCashValues = new List<int>();
        
        

        return await _client.GetOptionChainsAsync(tradierOptionChainSpecParams);
    }
    
    public async Task<OptionsData> GetMaxPainCalculationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        // user will enter ticker symbol, then dropdown of expirations (need another endpoint for this) will
        // appear. user chooses expiration, then the GetOptionChainAsync has the needed parameters.

        var ticker = tradierOptionChainSpecParams.Symbol;
        
        return await _client.GetOptionChainsAsync(tradierOptionChainSpecParams);
    }
}