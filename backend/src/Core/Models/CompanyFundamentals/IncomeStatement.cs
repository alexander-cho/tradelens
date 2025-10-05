namespace Core.Models.CompanyFundamentals;

public class IncomeStatement
{
    public required string Symbol { get; set; }
    public required IEnumerable<IncomeStatementPeriod> PeriodData { get; set; }
}

public class IncomeStatementPeriod
{
    public required string FiscalYear { get; set; }
    public required string Period { get; set; }
    public long Revenue { get; set; }
    public long GrossProfit { get; set; }
    public long NetIncome { get; set; }
}