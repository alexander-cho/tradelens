using Core.DTOs.Options;
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

    public CallsAndPutsCashSums CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        // this should return a hashmap containing cash values for calls, puts, and the max pain strike.
        // Each strike price is considered as a hypothetical closing price at expiry

        var optionChain = _client.GetOptionChainsAsync(tradierOptionChainSpecParams).Result;

        var calls = MaxPainHelpers.CalculateCallCashValues(optionChain);
        var puts = MaxPainHelpers.CalculatePutCashValues(optionChain);
        var totals = MaxPainHelpers.CalculateCashValuesTotal(optionChain);
        var maxPainValue = MaxPainHelpers.CalculateMaxPain(optionChain);
        
        _logger.LogInformation("Retrieved max pain data for ticker: {tradierOptionChainSpecParams.Symbol} expiring {tradierOptionChainSpecParams.Expiration}", tradierOptionChainSpecParams.Symbol, tradierOptionChainSpecParams.Expiration);

        return new CallsAndPutsCashSums
        {
            CallCashSums = calls,
            PutCashSums = puts,
            TotalCashSums = totals,
            MaxPainValue = maxPainValue
        };
    }
}