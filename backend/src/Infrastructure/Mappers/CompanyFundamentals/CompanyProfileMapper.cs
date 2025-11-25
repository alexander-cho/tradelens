using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class CompanyProfileMapper
{
    public static IEnumerable<CompanyProfile> ToCompanyProfile(IEnumerable<CompanyProfileDto> companyProfileDtos)
    {
        return companyProfileDtos.Select(dto => new CompanyProfile
        {
            Symbol = dto.Symbol,
            Price = dto.Price,
            MarketCap = dto.MarketCap,
            LastDividend = dto.LastDividend,
            Range = dto.Range,
            Change = dto.Change,
            ChangePercentage = dto.ChangePercentage,
            Volume = dto.Volume,
            Exchange = dto.Exchange,
            Website = dto.Website,
            Image = dto.Image
        });
    }
}