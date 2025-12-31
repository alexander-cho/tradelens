using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class FinancialRatiosMapper
{
    public static FinancialRatiosTtm? ToFinancialRatiosTtm(IEnumerable<FinancialRatiosTtmDto>? dtos)
    {
        var dto = dtos?.FirstOrDefault();
        if (dto == null) return null;

        return new FinancialRatiosTtm
        {
            Symbol = dto.Symbol,
            DebtToEquityRatioTtm = dto.DebtToEquityRatioTtm,
            GrossProfitMarginTtm = dto.GrossProfitMarginTtm,
            PriceToEarningsRatioTtm = dto.PriceToEarningsRatioTtm,
            ForwardPriceToEarningsGrowthRatioTtm = dto.ForwardPriceToEarningsGrowthRatioTtm,
            PriceToSalesRatioTtm = dto.PriceToSalesRatioTtm,
            PriceToBookRatioTtm = dto.PriceToBookRatioTtm,
            PriceToFreeCashFlowRatioTtm = dto.PriceToFreeCashFlowRatioTtm
        };
    }
}