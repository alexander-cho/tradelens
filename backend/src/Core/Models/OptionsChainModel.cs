namespace Core.Models;

public class OptionsChainModel
{
    public required FullOptionsChain Options { get; set; }
}

public class FullOptionsChain
{
    public required List<StrikePriceData> Option { get; set; }
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
    public Greeks? Greeks { get; set; }
}

public class Greeks
{
    public double Delta { get; set; }
    public double Gamma { get; set; }
    public double Theta { get; set; }
    public double Vega { get; set; }
    public double Rho { get; set; }
    public double Phi { get; set; }
    public double SmvVol { get; set; }
    public string? UpdatedAt { get; set; }
}