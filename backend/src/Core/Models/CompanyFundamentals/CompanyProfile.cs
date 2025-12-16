namespace Core.Models.CompanyFundamentals;

public class CompanyProfile
{
    public required string Symbol { get; set; }
    public float Price { get; set; }
    public double MarketCap { get; set; }
    public float Beta { get; set; }
    public float LastDividend { get; set; }
    public string? Range { get; set; }
    public float Change { get; set; }
    public float ChangePercentage { get; set; }
    public double Volume { get; set; }
    public double AverageVolume { get; set; }
    public string? CompanyName { get; set; }
    public string? Currency { get; set; }
    public string? Cik { get; set; }
    public string? Isin { get; set; }
    public string? Cusip { get; set; }
    public string? ExchangeFullName { get; set; }
    public string? Exchange { get; set; }
    public string? Industry { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public string? Ceo { get; set; }
    public string? Sector { get; set; }
    public string? Country { get; set; }

    // FMP returns this as a string, not an integer
    public string? FullTimeEmployees { get; set; }

    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Image { get; set; }
    public string? IpoDate { get; set; }

    public bool DefaultImage { get; set; }
    public bool IsEtf { get; set; }
    public bool IsActivelyTrading { get; set; }
    public bool IsAdr { get; set; }
    public bool IsFund { get; set; }
}