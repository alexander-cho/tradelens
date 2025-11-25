namespace Core.Models.CompanyFundamentals;

public class KeyMetricsTtm
{
    public required string Symbol { get; set; }
    public double EnterpriseValueTtm { get; set; }
    public decimal ReturnOnInvestedCapitalTtm { get; set; }
    public decimal CurrentRatio { get; set; }
    public decimal NetDebtToEbitdaTtm { get; set; }
}