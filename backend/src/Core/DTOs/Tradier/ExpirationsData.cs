namespace Core.DTOs.Tradier;

public class ExpiryData
{
    public required FullExpiryList Expirations { get; set; }
}

public class FullExpiryList
{
    public required List<string> Date { get; set; }
}