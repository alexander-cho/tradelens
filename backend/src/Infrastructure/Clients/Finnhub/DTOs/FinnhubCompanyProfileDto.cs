namespace Infrastructure.Clients.Finnhub.DTOs;

public record FinnhubCompanyProfileDto(
    string? Country,
    string? Currency,
    string? EstimateCurrency,
    string? Exchange,
    string? FinnhubIndustry,
    string? Ipo,
    string? Logo,
    double MarketCapitalization,
    string? Name,
    string? Phone,
    double ShareOutstanding,
    string? Ticker,
    string? Weburl
);