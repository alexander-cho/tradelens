using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Infrastructure.Clients.Tradier;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class MaxPainService : IMaxPainService
{
    private readonly ITradierClient _tradierClient;
    private readonly ILogger<MaxPainService> _logger;

    public MaxPainService(ITradierClient client, ILogger<MaxPainService> logger)
    {
        this._tradierClient = client;
        this._logger = logger;
    }

    // public CallsAndPutsCashSums CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    // {
    //     // this should return a hashmap containing cash values for calls, puts, and the max pain strike.
    //     // Each strike price is considered as a hypothetical closing price at expiry
    //
    //     var optionChain = _tradierClient.GetOptionsChainAsync(tradierOptionChainSpecParams).Result;
    //
    //     var calls = MaxPainCalculation.CalculateCallCashValues(optionChain);
    //     var puts = MaxPainCalculation.CalculatePutCashValues(optionChain);
    //     var totals = MaxPainCalculation.CalculateCashValuesTotal(optionChain);
    //     var maxPainValue = MaxPainCalculation.CalculateMaxPain(optionChain);
    //     
    //     _logger.LogInformation("Retrieved max pain data for ticker: {tradierOptionChainSpecParams.Symbol} expiring {tradierOptionChainSpecParams.Expiration}", tradierOptionChainSpecParams.Symbol, tradierOptionChainSpecParams.Expiration);
    //
    //     return new CallsAndPutsCashSums
    //     {
    //         CallCashSums = calls,
    //         PutCashSums = puts,
    //         TotalCashSums = totals,
    //         MaxPainValue = maxPainValue
    //     };
    // }
}