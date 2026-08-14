namespace Tradelens.Infrastructure.Clients.Fmp.DTOs;

public record BalanceSheetDto(
    string Symbol,
    string FiscalYear,
    string Period,
    long TotalAssets,
    long TotalLiabilities,
    long TotalStockHoldersEquity
    // debts -> types of debts
);