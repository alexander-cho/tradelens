namespace Tradelens.Infrastructure.Clients.Polygon.DTOs;

public record BarAggregatesDto(
    string Ticker,
    int QueryCount,
    int ResultsCount,
    bool Adjusted,
    List<Bar> Results
);

public record Bar(
    double V, // type double to handle number as scientific notation as defined by Polygon
    decimal Vw,
    decimal O,
    decimal C,
    decimal H,
    decimal L,
    long T,
    int N
);