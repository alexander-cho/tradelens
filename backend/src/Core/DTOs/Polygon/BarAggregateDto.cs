namespace Core.DTOs.Polygon;

public class BarAggregateDto
{
    public required string Ticker { get; set; }

    public int QueryCount { get; set; }

    public int ResultsCount { get; set; }

    public bool Adjusted { get; set; } = true;

    public required List<Bar> Results { get; set; }
}

public class Bar
{
    public double V { get; set; }  // type double to handle number as scientific notation as defined by Polygon
    public decimal Vw { get; set; }
    public decimal O { get; set; }
    public decimal C { get; set; }
    public decimal H { get; set; }
    public decimal L { get; set; }
    public long T { get; set; }
    public int N { get; set; }
}