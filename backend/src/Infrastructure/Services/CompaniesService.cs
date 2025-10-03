using Core.Interfaces;
using Core.Models;
using Infrastructure.Clients.Polygon;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public class CompaniesService : ICompaniesService
{
    private readonly IPolygonClient _polygonClient;

    public CompaniesService(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }

    public async Task<RelatedCompaniesModel> GetRelatedCompaniesAsync(string ticker)
    {
        var relatedCompaniesDto = await _polygonClient.GetRelatedCompaniesAsync(ticker);
        if (relatedCompaniesDto == null)
        {
            throw new InvalidOperationException($"Related companies data for {ticker} was not available");
        }

        var relatedCompanies = RelatedCompaniesMapper.ToRelatedCompaniesDomainModel(relatedCompaniesDto);

        return relatedCompanies;
    }
}