using Core.Models;

namespace Core.Interfaces;

public interface ICompaniesService
{
    Task<RelatedCompaniesModel> GetRelatedCompaniesAsync(string ticker);
}