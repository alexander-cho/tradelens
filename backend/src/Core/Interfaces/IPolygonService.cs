using Core.DTOs.Polygon;
using Core.Specifications;

namespace Core.Interfaces;

public interface IPolygonService
{
    Task<BarAggregateDto> GetBarAggregatesAsync(PolygonBarAggSpecParams polygonBarAggSpecParams);
    Task<RelatedCompaniesDto> GetRelatedCompaniesAsync(string ticker);
}
