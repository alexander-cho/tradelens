using Tradelens.Core.Specifications;
using Tradelens.Infrastructure.Clients.Polygon.DTOs;

namespace Tradelens.Infrastructure.Clients.Polygon;

public interface IPolygonClient
{
    Task<BarAggregatesDto?> GetBarAggregatesAsync(PolygonBarAggQueryParams polygonBarAggSpecParams);
    Task<RelatedCompaniesDto?> GetRelatedCompaniesAsync(string ticker);
}