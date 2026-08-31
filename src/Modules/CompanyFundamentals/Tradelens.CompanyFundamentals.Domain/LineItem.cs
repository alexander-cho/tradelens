namespace Tradelens.CompanyFundamentals.Domain;

public class LineItem
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    // reference to corresponding Filing object via filing_id probably
    public required string AsReported { get; set; }
    public required string Metric { get; set; }
    public required double Value { get; set; }
    public DateTime AddedAt { get; init; } = DateTime.UtcNow;
    
    private readonly List<object> _events = new List<object>();

    public List<object> GetEvents()
    {
        return _events.ToList();
    }
    //
    // public LineItem(string asReported, string metric)
    // {
    //     AsReported = asReported;
    //     Metric = metric;
    // }

    public void FlagLineItem()
    {
        _events.Add(new LineItemFlagged(this.Id, Guid.CreateVersion7(), DateTime.UtcNow));
    }
}

public record LineItemFlagged(Guid LineItemId, Guid FlaggerId, DateTime AddedAt);