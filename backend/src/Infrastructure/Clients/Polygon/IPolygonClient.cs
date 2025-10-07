using Core.Specifications;
using Infrastructure.Clients.Polygon.DTOs;

namespace Infrastructure.Clients.Polygon;

public interface IPolygonClient
{
    Task<BarAggregatesDto?> GetBarAggregatesAsync(PolygonBarAggSpecParams polygonBarAggSpecParams);
    Task<RelatedCompaniesDto?> GetRelatedCompaniesAsync(string ticker);
}