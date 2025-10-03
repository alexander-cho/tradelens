namespace Infrastructure.Clients.Finnhub.DTOs;

public record MarketStatusDto(
    string Exchange,
    string? Holiday,
    bool IsOpen,
    string Session,
    string Timezone,
    double T
);