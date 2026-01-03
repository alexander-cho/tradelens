namespace Tradelens.Infrastructure.Clients.Fmp.DTOs;

// actual values optional in case earnings haven't happened yet

public record EarningsCalendarDto(
    string Symbol,
    string Date,
    float? EpsActual,
    float EpsEstimated,
    float? RevenueActual,
    float RevenueEstimated
);