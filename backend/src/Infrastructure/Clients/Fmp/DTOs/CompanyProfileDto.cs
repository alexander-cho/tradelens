namespace Infrastructure.Clients.Fmp.DTOs;

public record CompanyProfileDto(
    string Symbol,
    float Price,
    double MarketCap,
    float LastDividend,
    string Range,
    float Change,
    float ChangePercentage,
    int Volume,
    string Exchange,
    string Website,
    string Image
);