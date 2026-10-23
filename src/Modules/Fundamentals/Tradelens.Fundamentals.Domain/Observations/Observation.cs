namespace Tradelens.Fundamentals.Domain.Observations;

/// <summary>
/// 
/// </summary>
public class Observation
{
    public Guid Id { get; } = Guid.CreateVersion7();
    // public FinancialReport? FinancialReport { get; }
    public string? AsReported { get; private set; }
    public string Metric { get; private set; }
    public double Figure { get; private set; }
    public DateTime AddedAt { get; init; } = DateTime.UtcNow;

    private readonly List<object> _events = new List<object>();

    private Observation(string? asReported, string metric, double figure)
    {
        AsReported = asReported;
        Metric = metric;
        Figure = figure;

        _events.Add(new ObservationAddedEvent(Id, AddedAt));
    }

    public static Observation Create(string? asReported, string metric, double figure)
    {
        return new Observation(asReported, metric, figure);
    }

    // // different creation methods, depending on if it came from an investor presentation or has iXBRL taxonomy from
    // // SEC with the asReported metric name
    // public static Observation CreateFromSecParse(string asReported, string metric, double figure)
    // {
    //     return new Observation(asReported, metric, figure);
    // }

    public List<object> GetEvents()
    {
        return _events.ToList();
    }

    // think about whether this action should be in the Audit bounded context rather
    // i.e., create an Audit (agg. root) object, e.g. IAudit audit = Audit.Create(typeof());
    public void FlagObservation()
    {
        _events.Add(new ObservationFlagged(Id, Guid.CreateVersion7(), DateTime.UtcNow));
    }
}

public record ObservationAddedEvent(Guid ObservationId, DateTime AddedAt);

public record ObservationFlagged(Guid ObservationId, Guid FlaggerId, DateTime AddedAt);