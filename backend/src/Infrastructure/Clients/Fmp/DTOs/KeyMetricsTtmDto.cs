namespace Infrastructure.Clients.Fmp.DTOs;

public record KeyMetricsTtmDto(
    string Symbol,
    double EnterpriseValueTtm,
    decimal ReturnOnInvestedCapitalTtm,
    decimal CurrentRatio,
    decimal NetDebtToEbitdaTtm
);