using System.Text.Json.Serialization;

namespace Core.DTOs.Tradier;

public class OptionsData
{
    public required FullOptionChain Options { get; set; }
}

public class FullOptionChain
{
    public required List<StrikePriceData> Option { get; set; }
}

public class StrikePriceData
{
    public required string Symbol { get; set; }
    public string? Description { get; set; }
    public required string Underlying { get; set; }
    public float Strike { get; set; }
    
    [JsonPropertyName("open_interest")]
    public int OpenInterest { get; set; }
    
    [JsonPropertyName("expiration_date")]
    public required string ExpirationDate { get; set; }
    
    [JsonPropertyName("option_type")]
    public required string OptionType { get; set; }
}

// public class Greeks
// {
//     public required double Delta { get; set; }
// }