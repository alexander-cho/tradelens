using Tradelens.Core.Models.CompanyFundamentals;
using Tradelens.Infrastructure.Clients.Finnhub.DTOs;

namespace Tradelens.Infrastructure.Mappers;

public static class FinnhubCompanyProfileMapper
{
    public static FinnhubCompanyProfile ToDomainModel(FinnhubCompanyProfileDto dto)
    {
        return new FinnhubCompanyProfile
        {
            Country = dto.Country,
            Currency = dto.Currency,
            EstimateCurrency = dto.EstimateCurrency,
            Exchange = dto.Exchange,
            FinnhubIndustry = dto.FinnhubIndustry,
            Ipo = dto.Ipo,
            Logo = dto.Logo,
            MarketCapitalization = dto.MarketCapitalization * 1_000_000,
            Name = dto.Name,
            Phone = dto.Phone,
            ShareOutstanding = dto.ShareOutstanding * 1_000_000,
            Ticker = dto.Ticker,
            Weburl = dto.Weburl
        };
    }
}