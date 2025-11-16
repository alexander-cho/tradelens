namespace Core.Entities;

public class CompanyMetric : BaseEntity
{
    // foreign Key to Stock
    public required string Ticker { get; set; }
    public Stock Stock { get; set; } = null!;
    
    public required string Period { get; set; } // "Q1", "Q2", "Q3", "Q4", "FY"
    public required int Year { get; set; }
    public required string Interval { get; set; } // "quarterly", "annual"
    public required DateOnly PeriodEndDate { get; set; }
    
    public required string Metric { get; set; }
    public required string ParentMetric { get; set; }
    public required string Section { get; set; } // "Financials", "KPI"
    public required string SourcedFrom { get; set; } // "10-K", "10-Q"
    
    public required decimal Value { get; set; }
    public required string Unit { get; set; }
}