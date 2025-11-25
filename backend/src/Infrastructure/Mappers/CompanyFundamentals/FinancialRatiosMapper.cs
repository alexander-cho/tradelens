using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class FinancialRatiosMapper
{
    public static IEnumerable<FinancialRatiosTtm> ToFinancialRatiosTtm(IEnumerable<FinancialRatiosTtmDto> dtos)
    {
        return dtos.Select(dto => new FinancialRatiosTtm
        {
            Symbol = dto.Symbol,
            DebtToEquityRatioTtm = dto.DebtToEquityRatioTtm,
            GrossProfitMarginTtm = dto.GrossProfitMarginTtm,
            PriceToEarningsRatioTtm = dto.PriceToEarningsRatioTtm,
            ForwardPriceToEarningsGrowthRatioTtm = dto.ForwardPriceToEarningsGrowthRatioTtm,
            PriceToSalesRatioTtm = dto.PriceToSalesRatioTtm,
            PriceToBookRatioTtm = dto.PriceToBookRatioTtm,
            PriceToFreeCashFlowRatioTtm = dto.PriceToFreeCashFlowRatioTtm
        });
    }
}