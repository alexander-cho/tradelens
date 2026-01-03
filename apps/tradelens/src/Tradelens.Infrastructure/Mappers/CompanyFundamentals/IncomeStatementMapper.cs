using Tradelens.Core.Models.CompanyFundamentals;
using Tradelens.Infrastructure.Clients.Fmp.DTOs;

namespace Tradelens.Infrastructure.Mappers.CompanyFundamentals;

public static class IncomeStatementMapper
{
    public static IncomeStatement ToIncomeStatement(IEnumerable<IncomeStatementDto> incomeStatementDtoList, string symbol)
    {
        // List<IncomeStatementPeriod> listOfIncomeStatements = [];
        // foreach (var incomeStatement in incomeStatementDtoAsList)
        // {
        //     var incomeStatementPeriodAsNewModel = ToIncomeStatementPeriod(incomeStatement);
        //     listOfIncomeStatements.Add(incomeStatementPeriodAsNewModel);
        // }

        var listOfIncomeStatements = incomeStatementDtoList.Select(ToIncomeStatementPeriod).ToList();

        return new IncomeStatement
        {
            Symbol = symbol,
            PeriodData = listOfIncomeStatements
        };
    }

    private static IncomeStatementPeriod ToIncomeStatementPeriod(IncomeStatementDto incomeStatementDto)
    {
        return new IncomeStatementPeriod
        {
            FiscalYear = incomeStatementDto.FiscalYear,
            Period = incomeStatementDto.Period,
            Revenue = incomeStatementDto.Revenue,
            GrossProfit = incomeStatementDto.GrossProfit,
            NetIncome = incomeStatementDto.NetIncome
        };
    }
}