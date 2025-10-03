using Core.Models;
using Infrastructure.Clients.Polygon.DTOs;

namespace Infrastructure.Mappers;

public static class RelatedCompaniesMapper
{
    public static RelatedCompaniesModel ToRelatedCompaniesDomainModel(RelatedCompaniesDto relatedCompaniesDto)
    {
        return new RelatedCompaniesModel
        {
            Ticker = relatedCompaniesDto.Ticker,
            Status = relatedCompaniesDto.Status,
            // Results is defined as nullable
            Results = relatedCompaniesDto.Results?.Select(ToRelatedCompanyDomainModel).ToList()
        };
    }

    private static Core.Models.RelatedCompany ToRelatedCompanyDomainModel(Clients.Polygon.DTOs.RelatedCompany relatedCompany)
    {
        return new Core.Models.RelatedCompany
        {
            Ticker = relatedCompany.Ticker
        };
    }
}