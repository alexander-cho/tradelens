namespace Tradelens.Core.Models;

public class OptionsChainModel
{
    public required List<StrikePriceData> OptionsChain { get; set; }
}

public class StrikePriceData
{
    public required string Symbol { get; set; }
    public string? Description { get; set; }
    public string? Underlying { get; set; }
    public float Strike { get; set; }
    public int Volume { get; set; }
    public int OpenInterest { get; set; }
    public string? ExpirationDate { get; set; }
    public string? OptionType { get; set; }
    public float? Last { get; set; }
    public float? Bid { get; set; }
    public float? Ask { get; set; }
    public ImpliedVolatility? ImpliedVolatility { get; set; }
    public Greeks? Greeks { get; set; }
    public Activity? Activity { get; set; }
}

public class Greeks
{
    public double Delta { get; set; }
    public double Gamma { get; set; }
    public double Theta { get; set; }
    public double Vega { get; set; }
    public double Rho { get; set; }
    public double Phi { get; set; }
    public string? UpdatedAt { get; set; }
}

public class ImpliedVolatility
{
    public double? BidIv { get; set; }
    public double? MidIv { get; set; }
    public double? AskIv { get; set; }
    public double? SmvVol { get; set; }
    public string? UpdatedAt { get; set; }
}

public class Activity
{
    public int Volume { get; set; }
    public int OpenInterest { get; set; }
    public double UnusualActivity => OpenInterest > 0 ? (double)Volume / OpenInterest : 0;
}