using Tradelens.Core.Models.CompanyFundamentals;
using Tradelens.Infrastructure.Clients.Fmp.DTOs;

namespace Tradelens.Infrastructure.Mappers.CompanyFundamentals;

public static class KeyMetricsMapper
{
    public static KeyMetricsTtm? ToKeyMetricsTtm(IEnumerable<KeyMetricsTtmDto>? dtos)
    {
        var dto = dtos?.FirstOrDefault();
        if (dto == null) return null;

        return new KeyMetricsTtm
        {
            Symbol = dto.Symbol,
            EnterpriseValueTtm = dto.EnterpriseValueTtm,
            ReturnOnInvestedCapitalTtm = dto.ReturnOnInvestedCapitalTtm,
            EvToEbitdaTtm = dto.EvToEbitdaTtm,
            NetDebtToEbitdaTtm = dto.NetDebtToEbitdaTtm
        };
    }

}