using Core.DTOs.Tradier;
using Core.Specifications;

namespace Core.Interfaces;

public interface IMaxPainService
{
    Task<OptionsData> GetMaxPainCalculationAsync(TradierOptionChainSpecParams tradierOptionChainSpecParams);
}