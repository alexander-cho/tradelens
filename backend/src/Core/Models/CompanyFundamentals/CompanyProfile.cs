namespace Core.Models.CompanyFundamentals;

public class CompanyProfile
{
    public required string Symbol { get; set; }
    public float Price { get; set; }
    public double MarketCap { get; set; }
    public float LastDividend { get; set; }
    public string? Range { get; set; }
    public float Change { get; set; }
    public float ChangePercentage { get; set; }
    public int Volume { get; set; }
    public string? Exchange { get; set; }
    public string? Website { get; set; }
    public string? Image { get; set; }
}