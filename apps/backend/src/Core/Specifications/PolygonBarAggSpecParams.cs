namespace Core.Specifications;

public class PolygonBarAggSpecParams
{
    public required string Ticker { get; set; }
    public int Multiplier { get; set; }
    public required string Timespan { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
}