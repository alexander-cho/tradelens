namespace Tradelens.Infrastructure.Clients.Tradier.DTOs;

public record ExpirationsDto(
    FullExpiryList Expirations
);

public record FullExpiryList(
    List<string> Date
);