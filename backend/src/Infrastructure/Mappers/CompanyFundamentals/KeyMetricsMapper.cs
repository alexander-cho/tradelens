using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class KeyMetricsMapper
{
    public static IEnumerable<KeyMetricsTtm> ToKeyMetricsTtm(IEnumerable<KeyMetricsTtmDto> dtos)
    {
        return dtos.Select(dto => new KeyMetricsTtm
        {
            Symbol = dto.Symbol,
            EnterpriseValueTtm = dto.EnterpriseValueTtm,
            ReturnOnInvestedCapitalTtm = dto.ReturnOnInvestedCapitalTtm,
            CurrentRatio = dto.CurrentRatio,
            NetDebtToEbitdaTtm = dto.NetDebtToEbitdaTtm
        });
    }
}