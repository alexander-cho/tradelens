namespace Infrastructure.Clients.Fmp.DTOs;

public record CongressTradeDto(
    string DisclosureDate,
    string TransactionDate,
    string Office,
    string? Symbol,
    string? Owner,
    string? AssetDescription,
    string? AssetType,
    string? Type,
    string? Amount,
    string? Link
);