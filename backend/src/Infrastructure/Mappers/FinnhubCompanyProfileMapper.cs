using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Finnhub.DTOs;

namespace Infrastructure.Mappers;

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
            MarketCapitalization = dto.MarketCapitalization,
            Name = dto.Name,
            Phone = dto.Phone,
            ShareOutstanding = dto.ShareOutstanding,
            Ticker = dto.Ticker,
            Weburl = dto.Weburl
        };
    }
}