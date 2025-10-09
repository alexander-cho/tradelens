using Core.Interfaces;
using Core.Models;
using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp;
using Infrastructure.Clients.Polygon;
using Infrastructure.Mappers;
using Infrastructure.Mappers.CompanyFundamentals;

namespace Infrastructure.Services;

public class CompanyFundamentalsService : ICompanyFundamentalsService
{
    private readonly IPolygonClient _polygonClient;
    private readonly IFmpClient _fmpClient;

    public CompanyFundamentalsService(IPolygonClient polygonClient, IFmpClient fmpClient)
    {
        _polygonClient = polygonClient;
        _fmpClient = fmpClient;
    }

    public async Task<RelatedCompaniesModel> GetRelatedCompaniesAsync(string ticker)
    {
        var relatedCompaniesDto = await _polygonClient.GetRelatedCompaniesAsync(ticker);
        if (relatedCompaniesDto == null)
        {
            throw new InvalidOperationException($"Related companies data for {ticker} was not available");
        }

        var relatedCompanies = RelatedCompaniesMapper.ToRelatedCompaniesDomainModel(relatedCompaniesDto);

        return relatedCompanies;
    }

    public async Task<IncomeStatement> GetIncomeStatementAsync(string ticker, int limit, string period)
    {
        var incomeStatementDto = await _fmpClient.GetIncomeStatementAsync(ticker, 5, period);
        var incomeStatements = IncomeStatementMapper.ToIncomeStatement(incomeStatementDto, ticker);

        return incomeStatements;
    }
    
    public async Task<BalanceSheet> GetBalanceSheetAsync(string ticker, int limit, string period)
    {
        var balanceSheetDto = await _fmpClient.GetBalanceSheetStatementAsync(ticker, 5, period);
        var balanceSheets = BalanceSheetMapper.ToBalanceSheet(balanceSheetDto, ticker);

        return balanceSheets;
    }
    
    public async Task<CashFlowStatement> GetCashFlowStatementAsync(string ticker, int limit, string period)
    {
        var cashFlowStatementDto = await _fmpClient.GetCashFlowStatementAsync(ticker, 5, period);
        var cashFlowStatements = CashFlowStatementMapper.ToCashFlowStatement(cashFlowStatementDto, ticker);

        return cashFlowStatements;
    }
}