using Tradelens.Core.Models.CompanyFundamentals;
using Tradelens.Infrastructure.Clients.Fmp.DTOs;

namespace Tradelens.Infrastructure.Mappers.CompanyFundamentals;

public static class BalanceSheetMapper
{
    public static BalanceSheet ToBalanceSheet(IEnumerable<BalanceSheetDto> balanceSheetDtoList, string symbol)
    {
        var listOfBalanceSheets = balanceSheetDtoList.Select(ToBalanceSheetPeriod).ToList();

        return new BalanceSheet
        {
            Symbol = symbol,
            PeriodData = listOfBalanceSheets
        };
    }
    
    private static BalanceSheetPeriod ToBalanceSheetPeriod(BalanceSheetDto balanceSheetDto)
    {
        return new BalanceSheetPeriod
        {
            FiscalYear = balanceSheetDto.FiscalYear,
            Period = balanceSheetDto.Period,
            TotalAssets = balanceSheetDto.TotalAssets,
            TotalLiabilities = balanceSheetDto.TotalLiabilities,
            TotalStockholdersEquity = balanceSheetDto.TotalStockHoldersEquity
        };
    }
}