namespace Core.Models.CompanyFundamentals;

public class CashFlowStatement
{
    public required string Symbol { get; set; }
    public required IEnumerable<CashFlowStatementPeriod> PeriodData { get; set; }
}

public class CashFlowStatementPeriod
{
    public required string FiscalYear { get; set; }
    public required string Period { get; set; }
    public long StockBasedCompensation { get; set; }
    public long FreeCashFlow { get; set; }
}