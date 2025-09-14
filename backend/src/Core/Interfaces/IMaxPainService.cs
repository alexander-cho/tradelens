using Core.DTOs.Options;
using Core.DTOs.Tradier;
using Core.Specifications;

namespace Core.Interfaces;

public interface IMaxPainService
{
    CallsAndPutsCashSums CalculateCashValuesForOneExpirationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
    
    Task<OptionsData> GetMaxPainCalculationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
}