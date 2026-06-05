using System.Text.Json.Serialization;

namespace Tradelens.Infrastructure.Clients.Polygon.DTOs;

public record RelatedCompaniesDto(
    [property: JsonPropertyName("stock_symbol")]
    string StockSymbol,
    string? Status,
    List<RelatedCompany>? Results
);

public record RelatedCompany(
    string Ticker
);