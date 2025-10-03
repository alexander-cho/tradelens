namespace Core.Interfaces;

public interface IMarketDataService
{
    Task<string> GetMarketStatusAsync();
}