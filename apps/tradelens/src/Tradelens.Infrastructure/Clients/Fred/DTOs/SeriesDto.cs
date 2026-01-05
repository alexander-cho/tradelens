namespace Tradelens.Infrastructure.Clients.Fred.DTOs;

public record SeriesDto(
    IEnumerable<SeriesDataDto> Seriess
);

public record SeriesDataDto(
    string Id,
    string Title,
    string Frequency,
    string Units
);