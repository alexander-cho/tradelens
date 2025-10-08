namespace Infrastructure.Clients.Fmp.DTOs;

public record BalanceSheetDto(
    string Symbol,
    string FiscalYear,
    string Period,
    long TotalAssets
    // debts -> types of debts
);