namespace Infrastructure.Clients.Polygon.DTOs;

public record RelatedCompaniesDto(
    string Ticker,
    string? Status,
    List<RelatedCompany>? Results
);

public record RelatedCompany(
    string Ticker
);