namespace Tradelens.Core.Models.CompanyFundamentals;

public class FinancialRatiosTtm
{
    public required string Symbol { get; set; }
    public decimal DebtToEquityRatioTtm { get; set; }
    public decimal GrossProfitMarginTtm { get; set; }
    public decimal PriceToEarningsRatioTtm { get; set; }
    public decimal ForwardPriceToEarningsGrowthRatioTtm { get; set; }
    public decimal PriceToSalesRatioTtm { get; set; }
    public decimal PriceToBookRatioTtm { get; set; }
    public decimal PriceToFreeCashFlowRatioTtm { get; set; }
}