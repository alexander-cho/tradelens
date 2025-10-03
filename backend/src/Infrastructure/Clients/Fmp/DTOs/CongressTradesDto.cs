namespace Infrastructure.Clients.Fmp.DTOs;

public record CongressTradesDto(
    string DisclosureDate,
    string TransactionDate,
    string Office,
    string? Symbol = null,
    string? Owner = null,
    string? AssetDescription = null,
    string? AssetType = null,
    string? Type = null,
    string? Amount = null,
    string? Link = null
);