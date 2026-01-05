namespace Tradelens.Core.Constants;

public static class CompanyFundamentalMetricMappings
{
    public static readonly Dictionary<string, string> MetricToEndpoint = new()
    {
        { "revenue", "IncomeStatement" },
        { "netIncome", "IncomeStatement" },
        { "grossProfit", "IncomeStatement" },

        { "totalAssets", "BalanceSheet" },
        { "totalLiabilities", "BalanceSheet" },
        { "totalStockholdersEquity", "BalanceSheet" },

        { "freeCashFlow", "CashFlow" },
        { "stockBasedCompensation", "CashFlow" },
        { "cashAtEndOfPeriod", "CashFlow" }
    };
}