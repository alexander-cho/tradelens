namespace Core.Models;

public class FredModels
{
}

public class MarginBalanceModel
{
    public required string ObservationStart { get; init; }
    public required string ObservationEnd { get; init; }
    public required string Units { get; init; }
    public int Count { get; init; }
    public IReadOnlyList<DataPointModel> Observations { get; init; } = new List<DataPointModel>();
}

public class MoneyMarketFundsModel
{
    public required string ObservationStart { get; init; }
    public required string ObservationEnd { get; init; }
    public required string Units { get; init; }
    public int Count { get; init; }
    public IReadOnlyList<DataPointModel> Observations { get; init; } = new List<DataPointModel>();
}

public class DataPointModel
{
    public required string Date { get; init; }

    // value of the observation. Nullable because the API returns ".".
    // multiply by 1_000_000 since it's in millions?
    public decimal? Value { get; init; }

    public required string RealtimeStart { get; init; }
    public required string RealtimeEnd { get; init; }
}