namespace Core.Interfaces;

public interface ICompaniesService
{
    Task<string> GetRelatedCompaniesAsync(string ticker);
}