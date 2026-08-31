namespace Tradelens.CompanyFundamentals.Domain;

public class FinancialReport
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    // reference to Company entity by id probably
    // possibly consider value objects for some of the following
    public required string FilingType { get; set; }  // e.g. 10-Q, 8-K, 10-K
    public required string FiscalYear { get; set; }  // e.g. 2026
    public required string Period { get; set; }  // e.g. Q1, FY
    public DateOnly PeriodStart { get; set; }  // e.g. 2026-01-01
    public DateOnly PeriodEnd { get; set; }  // e.g. 2026-03-31
    public DateOnly FilingDate { get; set; }  // e.g. 2026-07-29
    public DateTime AddedAt { get; init; }
    
    private readonly List<object> _events = new List<object>();
    
    public List<object> GetEvents()
    {
        return _events.ToList();
    }

    public void FlagFinancialReport()
    {
        _events.Add(new FinancialReportNotifiedAboutFlaggedLineItem(this.Id, Guid.CreateVersion7(), DateTime.UtcNow));
    }
}

public record FinancialReportNotifiedAboutFlaggedLineItem(Guid FinancialReportId, Guid LineItemId, DateTime AddedAt);