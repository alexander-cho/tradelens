using System.Text.Json.Serialization;

namespace Infrastructure.Clients.Tradier.DTOs;

/// <summary>
/// {options: {option: [{symbol: ...},{},{},...] } }
/// </summary>
public record OptionsChainDto(
    FullOptionsChain Options
);

public record FullOptionsChain(
    List<StrikePriceData> Option
);

public record StrikePriceData(
    string Symbol,
    string? Description,
    string Underlying,
    float Strike,
    [property: JsonPropertyName("open_interest")]
    int OpenInterest,
    [property: JsonPropertyName("expiration_date")]
    string ExpirationDate,
    [property: JsonPropertyName("option_type")]
    string OptionType
);

// public record Greeks(
//     double Delta
// );