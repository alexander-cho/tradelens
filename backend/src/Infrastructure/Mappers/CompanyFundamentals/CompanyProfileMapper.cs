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
            Beta = dto.Beta,
            LastDividend = dto.LastDividend,
            Range = dto.Range,
            Change = dto.Change,
            ChangePercentage = dto.ChangePercentage,
            Volume = dto.Volume,
            AverageVolume = dto.AverageVolume,
            CompanyName = dto.CompanyName,
            Currency = dto.Currency,
            Cik = dto.Cik,
            Isin = dto.Isin,
            Cusip = dto.Cusip,
            ExchangeFullName = dto.ExchangeFullName,
            Exchange = dto.Exchange,
            Industry = dto.Industry,
            Website = dto.Website,
            Description = dto.Description,
            Ceo = dto.Ceo,
            Sector = dto.Sector,
            Country = dto.Country,
            FullTimeEmployees = dto.FullTimeEmployees,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Zip = dto.Zip,
            Image = dto.Image,
            IpoDate = dto.IpoDate,
            DefaultImage = dto.DefaultImage,
            IsEtf = dto.IsEtf,
            IsActivelyTrading = dto.IsActivelyTrading,
            IsAdr = dto.IsAdr,
            IsFund = dto.IsFund
        };
    }
}