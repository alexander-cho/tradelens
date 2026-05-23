namespace Tradelens.Core.Models;

public class FredModels
{
}

public class SeriesObservations
{
    public string? Id { get; init; }
    public string? Title { get; init; }
    public string? Frequency { get; init; }
    public string? Units { get; init; }
    public IReadOnlyList<DataPoint> Observations { get; init; } = new List<DataPoint>();
}

public class DataPoint
{
    public required string Date { get; init; }

    // value of the observation. Nullable because the Tradelens.Api returns ".".
    // multiply by 1_000_000 since it's in millions?
    public decimal? Value { get; init; }

    public required string RealtimeStart { get; init; }
    public required string RealtimeEnd { get; init; }
}