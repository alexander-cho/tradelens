using Core.DTOs.Polygon;
using Core.Specifications;

namespace Core.Interfaces;

public interface IPolygonClient
{
    Task<BarAggregateDto> GetBarAggregatesAsync(PolygonBarAggSpecParams polygonBarAggSpecParams);
    Task<RelatedCompaniesDto> GetRelatedCompaniesAsync(string ticker);
}