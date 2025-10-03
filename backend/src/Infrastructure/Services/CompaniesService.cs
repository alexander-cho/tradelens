using Core.Interfaces;
using Infrastructure.Clients.Polygon;

namespace Infrastructure.Services;

public class CompaniesService : ICompaniesService
{
    private readonly IPolygonClient _polygonClient;

    public CompaniesService(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }

    public async Task<string> GetRelatedCompaniesAsync(string ticker)
    {
        // return await _polygonClient.GetRelatedCompaniesAsync(ticker);

        throw new NotImplementedException();
    }
}