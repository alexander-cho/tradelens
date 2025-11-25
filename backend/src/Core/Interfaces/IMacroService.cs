using Core.Models;

namespace Core.Interfaces;

public interface IMacroService
{
    Task<MarginBalanceModel> GetMarginBalanceAsync();
    Task<MoneyMarketFundsModel> GetMoneyMarketFundsAsync();
}