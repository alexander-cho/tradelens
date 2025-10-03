namespace Core.Interfaces;

public interface IOptionsService
{
    public Task<string> GetExpiryListForUnderlyingAsync(string symbol);
}