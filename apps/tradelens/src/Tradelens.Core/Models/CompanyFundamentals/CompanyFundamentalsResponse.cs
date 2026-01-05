namespace Tradelens.Core.Models.CompanyFundamentals;

// all possible responses, nullable -> whichever ones user did not request.

// EXAMPLE STRUCTURE:
//{
//   metricName: "revenue",
//   data: [
//     { period: "Q2", fiscalYear: "2025", value: 1129512000 },
//     { period: "Q1", fiscalYear: "2025", value: 1036845000 },
//     ...
//   ]
// },

public class CompanyFundamentalsResponse
{
    // public IncomeStatement? IncomeStatement { get; set; }
    // public BalanceSheet? BalanceSheet { get; set; }
    // public CashFlowStatement? CashFlowStatement { get; set; }
    public required List<Metric> MetricData { get; set; }
}

public class Metric
{
    public required string MetricName { get; set; }
    public required List<ValueDataAtEachPeriod> Data { get; set; }
}

public class ValueDataAtEachPeriod
{
    public required string Period { get; set; }
    public required string FiscalYear { get; set; }
    public long Value { get; set; }
}