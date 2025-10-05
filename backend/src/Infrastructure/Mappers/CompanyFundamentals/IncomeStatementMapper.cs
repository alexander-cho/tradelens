using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers.CompanyFundamentals;

public static class IncomeStatementMapper
{
    public static IncomeStatement ToIncomeStatement(IEnumerable<IncomeStatementDto> incomeStatementDtoAsList, string symbol)
    {
        // List<IncomeStatementPeriod> listOfIncomeStatements = [];
        // foreach (var incomeStatement in incomeStatementDtoAsList)
        // {
        //     var incomeStatementPeriodAsNewModel = ToIncomeStatementPeriod(incomeStatement);
        //     listOfIncomeStatements.Add(incomeStatementPeriodAsNewModel);
        // }

        var listOfIncomeStatements = incomeStatementDtoAsList.Select(ToIncomeStatementPeriod).ToList();

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