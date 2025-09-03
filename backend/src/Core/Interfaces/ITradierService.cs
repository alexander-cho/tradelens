using Core.DTOs.Tradier;

namespace Core.Interfaces;

public interface ITradierService
{
    Task<ExpiryData> GetExpiryListForUnderlyingAsync(string symbol);
}