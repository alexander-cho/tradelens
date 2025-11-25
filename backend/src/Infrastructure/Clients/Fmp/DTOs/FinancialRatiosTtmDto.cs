namespace Infrastructure.Clients.Fmp.DTOs;

public record FinancialRatiosTtmDto(
    string Symbol,
    decimal DebtToEquityRatioTtm,
    decimal GrossProfitMarginTtm,
    
    decimal PriceToEarningsRatioTtm,
    decimal ForwardPriceToEarningsGrowthRatioTtm,
    decimal PriceToSalesRatioTtm,
    decimal PriceToBookRatioTtm,
    decimal PriceToFreeCashFlowRatioTtm
);