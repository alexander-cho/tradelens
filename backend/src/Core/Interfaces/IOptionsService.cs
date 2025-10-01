using Core.DTOs.Tradier;

namespace Core.Interfaces;

public interface IOptionsService
{
    public Task<ExpiryData> GetExpiryListForUnderlyingAsync(string symbol);
}