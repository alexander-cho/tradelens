using Core.Interfaces;
using Core.Models;
using Infrastructure.Clients.Fred;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public class MacroService : IMacroService
{
    private readonly IFredClient _fredClient;

    public MacroService(IFredClient fredClient)
    {
        _fredClient = fredClient;
    }
    
    public async Task<MarginBalanceModel> GetMarginBalanceAsync()
    {
        var marginBalanceDto = await _fredClient.GetMarginBalanceAsync();
        
        if (marginBalanceDto == null)
        {
            throw new InvalidOperationException("Market status data was not available");
        }

        var marginBalance = MarginBalanceMapper.ToMarginBalanceModel(marginBalanceDto);

        return marginBalance;
    }

    public async Task<MoneyMarketFundsModel> GetMoneyMarketFundsAsync()
    {
        var moneyMarketFundsDto = await _fredClient.GetMoneyMarketFundsAsync();
        
        if (moneyMarketFundsDto == null)
        {
            throw new InvalidOperationException("Market status data was not available");
        }

        var moneyMarketFunds = MoneyMarketFundsMapper.ToMoneyMarketFundsModel(moneyMarketFundsDto);

        return moneyMarketFunds;
    }
}