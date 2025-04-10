using Core.DTOs.Tradier;

namespace Core.Interfaces;

public interface IMaxPainService
{
    Task<OptionsData> GetOptionChainsAsync();
}