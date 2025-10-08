namespace Infrastructure.Clients.Fmp.DTOs;

public record CashFlowStatementDto(
    string Symbol,
    string FiscalYear,
    string Period,
    long StockBasedCompensation,
    long FreeCashFlow
);