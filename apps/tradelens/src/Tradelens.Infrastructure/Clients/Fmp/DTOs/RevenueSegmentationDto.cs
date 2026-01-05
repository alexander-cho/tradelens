namespace Tradelens.Infrastructure.Clients.Fmp.DTOs;

public record RevenueSegmentationDto(
    string Symbol,
    int FiscalYear,
    string Period,
    Dictionary<string, int> Data
);