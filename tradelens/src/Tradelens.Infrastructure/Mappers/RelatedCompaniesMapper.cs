using Tradelens.Core.Models;
using Tradelens.Infrastructure.Clients.Polygon.DTOs;

namespace Tradelens.Infrastructure.Mappers;

public static class RelatedCompaniesMapper
{
    public static List<string> ToRelatedCompanies(RelatedCompaniesModel model)
    {
        List<string> tickersList = [];
        
        if (model.Results != null)
        {
            foreach (var relatedCompany in model.Results)
            {
                var tickerToAdd = relatedCompany.Ticker;
                tickersList.Add(tickerToAdd);
            }
        }

        return tickersList;
    }
    
    public static RelatedCompaniesModel ToRelatedCompaniesDomainModel(RelatedCompaniesDto relatedCompaniesDto)
    {
        return new RelatedCompaniesModel
        {
            Ticker = relatedCompaniesDto.StockSymbol,
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