namespace Tradelens.Infrastructure.Clients.Fmp.DTOs;

public record IncomeStatementDto(
    string Symbol,
    string FiscalYear,
    string Period,
    long Revenue,
    long GrossProfit,
    long NetIncome
    // add more later
);
