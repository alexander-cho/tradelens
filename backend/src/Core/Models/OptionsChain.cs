namespace Core.Models;

public class OptionsChain
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
    public int OpenInterest { get; set; }
    public string? ExpirationDate { get; set; }
    public string? OptionType { get; set; }
}

// public class Greeks
// {
//     public double Delta { get; set; }
// }