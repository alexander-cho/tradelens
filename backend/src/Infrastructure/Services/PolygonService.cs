using Core.DTOs.Polygon;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class PolygonService : IPolygonService
{
    private readonly IPolygonClient _client;

    public PolygonService(IPolygonClient client)
    {
        this._client = client;
    }

    public async Task<BarAggregateDto> GetBarAggregatesAsync(PolygonBarAggSpecParams polygonBarAggSpecParams)
    {
        return await _client.GetBarAggregatesAsync(polygonBarAggSpecParams);
    }

    public async Task<RelatedCompaniesDto> GetRelatedCompaniesAsync(string ticker)
    {
        return await _client.GetRelatedCompaniesAsync(ticker);
    }
}