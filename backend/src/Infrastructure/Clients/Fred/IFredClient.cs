using Infrastructure.Clients.Fred.DTOs;

namespace Infrastructure.Clients.Fred;

public interface IFredClient
{
    Task<MarginBalanceDto?> GetMarginBalanceAsync();
    Task<MoneyMarketFundsDto?> GetMoneyMarketFundsAsync();
}