namespace Core.Interfaces;

public interface IPolygonService
{
    Task<string> GetBarAggregatesAsync();
    Task<string> GetRelatedCompaniesAsync();
}
