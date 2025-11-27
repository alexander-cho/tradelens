using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class CompanyProfileMapper
{
    public static CompanyProfile? ToCompanyProfile(IEnumerable<CompanyProfileDto>? dtos)
    {
        var dto = dtos?.FirstOrDefault();
        if (dto == null) return null;
    
        return new CompanyProfile
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
        };
    }
}