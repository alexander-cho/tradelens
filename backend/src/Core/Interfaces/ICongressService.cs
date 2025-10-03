namespace Core.Interfaces;

public interface ICongressService
{
    Task<IEnumerable<string>> GetCongressTradesAsync(string chamber);
}