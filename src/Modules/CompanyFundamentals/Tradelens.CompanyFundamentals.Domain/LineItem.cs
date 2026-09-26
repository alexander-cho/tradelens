namespace Tradelens.CompanyFundamentals.Domain;

public class LineItem
{
    public Guid Id { get; } = Guid.CreateVersion7();
    // reference to corresponding Filing object via filing_id probably
    public string AsReported { get; private set; }
    public string Metric { get; private set; }
    public double Value { get; private set; }
    public DateTime AddedAt { get; init; } = DateTime.UtcNow;
    
    private readonly List<object> _events = new List<object>();

    private LineItem(string asReported, string metric, double value)
    {
        AsReported = asReported;
        Metric = metric;
        Value = value;
    }

    public static LineItem Create(string asReported, string metric, double value)
    {
        return new LineItem(asReported, metric, value);
    }

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