using Core.DTOs.Tradier;

namespace Core.Interfaces;

public interface ITradierClient
{
    Task<OptionsData> GetOptionChainsAsync();
}