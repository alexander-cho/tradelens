namespace Tradelens.Core.Models.CompanyFundamentals;

public class BalanceSheet
{
    public required string Symbol { get; set; }
    public required IEnumerable<BalanceSheetPeriod> PeriodData { get; set; }
}

public class BalanceSheetPeriod
{
    public required string FiscalYear { get; set; }
    public required string Period { get; set; }
    public long TotalAssets { get; set; }
    public long TotalLiabilities { get; set; }
    public long TotalStockholdersEquity { get; set; }
}