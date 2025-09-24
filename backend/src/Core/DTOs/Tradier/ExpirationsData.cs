namespace Core.DTOs.Tradier;

// Tradier nested responses

public class ExpiryData
{
    public required FullExpiryList Expirations { get; set; }
}

public class FullExpiryList
{
    public required List<string> Date { get; set; }
}
