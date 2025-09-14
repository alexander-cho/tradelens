using Core.DTOs.Options;
using Core.DTOs.Tradier;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Helpers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class MaxPainService : IMaxPainService
{
    private readonly ITradierClient _client;
    private readonly ILogger<MaxPainService> _logger;

    public MaxPainService(ITradierClient client, ILogger<MaxPainService> logger)
    {
        this._client = client;
        this._logger = logger;
    }

    public List<CashSumAtPrice> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        // this should return a hashmap containing cash values for calls, puts, and the max pain strike.
        // Each strike price is considered as a hypothetical closing price at expiry
        
        // var ticker = tradierOptionChainSpecParams.Symbol;

        var optionChain = _client.GetOptionChainsAsync(tradierOptionChainSpecParams).Result;

        var callCashSums = CashValueHelpers.CalculateCallCashValues(optionChain);
        var putCashSums = CashValueHelpers.CalculatePutCashValues(optionChain);
        
        _logger.LogInformation("Call cash sums: {callCashSums}, and put cash sums: {putCashSums}", callCashSums, putCashSums);

        return callCashSums;
    }
    
    public async Task<OptionsData> GetMaxPainCalculationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        // user will enter ticker symbol, then dropdown of expirations (need another endpoint for this) will
        // appear. user chooses expiration, then the GetOptionChainAsync has the needed parameters.

        // var ticker = tradierOptionChainSpecParams.Symbol;
        
        return await _client.GetOptionChainsAsync(tradierOptionChainSpecParams);
    }
}