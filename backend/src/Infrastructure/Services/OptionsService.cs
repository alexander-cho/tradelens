using Core.Interfaces;
using Core.Models;
using Core.Services;
using Core.Specifications;
using Infrastructure.Clients.Tradier;
using Infrastructure.Mappers;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class OptionsService : IOptionsService
{
    private readonly ITradierClient _tradierClient;
    private readonly ILogger<OptionsService> _logger;

    public OptionsService(ITradierClient tradierClient, ILogger<OptionsService> logger)
    {
        _tradierClient = tradierClient;
        _logger = logger;
    }

    public async Task<OptionsChainModel> GetOptionsChainAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        var optionsChainDto = await _tradierClient.GetOptionsChainAsync(tradierOptionChainSpecParams);

        var optionsChain = OptionsChainMapper.ToOptionsChainDomainModel(optionsChainDto);

        return optionsChain;
    }
    
    public async Task<CallsAndPutsCashSums> CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        var optionsChainDto = await _tradierClient.GetOptionsChainAsync(tradierOptionChainSpecParams);

        var optionsChain = OptionsChainMapper.ToOptionsChainDomainModel(optionsChainDto);
    
        var calls = MaxPainCalculator.CalculateCallCashValues(optionsChain);
        var puts = MaxPainCalculator.CalculatePutCashValues(optionsChain);
        var totals = MaxPainCalculator.CalculateCashValuesTotal(optionsChain);
        var maxPainValue = MaxPainCalculator.CalculateMaxPain(optionsChain);
        
        _logger.LogInformation("Retrieved max pain data for ticker: {tradierOptionChainSpecParams.Symbol} expiring {tradierOptionChainSpecParams.Expiration}", tradierOptionChainSpecParams.Symbol, tradierOptionChainSpecParams.Expiration);
    
        return new CallsAndPutsCashSums
        {
            CallCashSums = calls,
            PutCashSums = puts,
            TotalCashSums = totals,
            MaxPainValue = maxPainValue
        };
    }

    public async Task<ExpirationsModel> GetExpiryListForUnderlyingAsync(string symbol)
    {
        var expirationsDto = await _tradierClient.GetExpiryDatesForUnderlyingAsync(symbol);

        var expirations = ExpirationsMapper.ToExpirationsDomainModel(expirationsDto);

        return expirations;
    }
}