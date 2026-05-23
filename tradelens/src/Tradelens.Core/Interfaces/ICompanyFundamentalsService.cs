using Tradelens.Core.Models;
using Tradelens.Core.Models.CompanyFundamentals;

namespace Tradelens.Core.Interfaces;

public interface ICompanyFundamentalsService
{
    Task<HashSet<string>> GetRelatedCompaniesAsync(string ticker);
    Task<CompanyFundamentalsResponse> GetCompanyFundamentalMetricsAsync(string ticker, string period, List<string> metric);
    Task<IncomeStatement> GetIncomeStatementAsync(string ticker, int limit, string period);
    Task<BalanceSheet> GetBalanceSheetAsync(string ticker, int limit, string period);
    Task<CashFlowStatement> GetCashFlowStatementAsync(string ticker, int limit, string period);
    Task<CompanyProfile> GetCompanyProfileDataAsync(string symbol);
    Task<KeyMetricsTtm> GetKeyMetricsTtmAsync(string symbol);
    Task<FinancialRatiosTtm> GetFinancialRatiosTtmAsync(string symbol);
    Task<FinnhubCompanyProfile> GetFinnhubCompanyProfileAsync(string symbol);
}