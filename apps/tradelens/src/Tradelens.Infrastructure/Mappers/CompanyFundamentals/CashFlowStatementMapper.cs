using Tradelens.Core.Models.CompanyFundamentals;
using Tradelens.Infrastructure.Clients.Fmp.DTOs;

namespace Tradelens.Infrastructure.Mappers.CompanyFundamentals;

public static class CashFlowStatementMapper
{
    public static CashFlowStatement ToCashFlowStatement(IEnumerable<CashFlowStatementDto> cashFlowStatementDtoList, string symbol)
    {
        var listOfCashFlowStatements = cashFlowStatementDtoList.Select(ToCashFlowStatementPeriod).ToList();
        
        return new CashFlowStatement
        {
            Symbol = symbol,
            PeriodData = listOfCashFlowStatements
        };
    }
    
    private static CashFlowStatementPeriod ToCashFlowStatementPeriod(CashFlowStatementDto cashFlowStatementDto)
    {
        return new CashFlowStatementPeriod
        {
            FiscalYear = cashFlowStatementDto.FiscalYear,
            Period = cashFlowStatementDto.Period,
            StockBasedCompensation = cashFlowStatementDto.StockBasedCompensation,
            FreeCashFlow = cashFlowStatementDto.FreeCashFlow,
            CashAtEndOfPeriod = cashFlowStatementDto.CashAtEndOfPeriod
        };
    }
}