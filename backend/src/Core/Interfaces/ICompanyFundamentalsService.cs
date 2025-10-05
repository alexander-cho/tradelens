using Core.Models;
using Core.Models.CompanyFundamentals;

namespace Core.Interfaces;

public interface ICompanyFundamentalsService
{
    Task<RelatedCompaniesModel> GetRelatedCompaniesAsync(string ticker);
    Task<IncomeStatement> GetIncomeStatementAsync(string ticker, int limit, string period);
}