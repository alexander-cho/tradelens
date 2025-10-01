using Core.DTOs.Polygon;

namespace Core.Interfaces;

public interface ICompaniesService
{
    Task<RelatedCompaniesDto?> GetRelatedCompaniesAsync(string ticker);
}