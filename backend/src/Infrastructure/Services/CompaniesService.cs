using Core.DTOs.Polygon;
using Core.Interfaces;

namespace Infrastructure.Services;

public class CompaniesService : ICompaniesService
{
    private readonly IPolygonClient _polygonClient;

    public CompaniesService(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }

    public async Task<RelatedCompaniesDto?> GetRelatedCompaniesAsync(string ticker)
    {
        return await _polygonClient.GetRelatedCompaniesAsync(ticker);
    }
}