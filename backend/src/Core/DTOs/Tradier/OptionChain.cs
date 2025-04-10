using System.Text.Json.Serialization;

namespace Core.DTOs.Tradier;

public class OptionsData
{
    public required FullOptionChain Options { get; set; }
}

public class FullOptionChain
{
    public required List<Strike> Option { get; set; }
}

public class Strike
{
    public required string Symbol { get; set; }
    public required string Description { get; set; }
    
    [JsonPropertyName("expiration_date")]
    public required string ExpirationDate { get; set; }
}

// public class Greeks
// {
//     public required double Delta { get; set; }
// }