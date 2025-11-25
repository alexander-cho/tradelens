using System.Text.Json.Serialization;

namespace Infrastructure.Clients.Fred.DTOs;

public record MarginBalanceDto(
    [property: JsonPropertyName("realtime_start")]
    string RealtimeStart,
    [property: JsonPropertyName("realtime_end")]
    string RealtimeEnd,
    [property: JsonPropertyName("observation_start")]
    string ObservationStart,
    [property: JsonPropertyName("observation_end")]
    string ObservationEnd,
    string Units,
    [property: JsonPropertyName("output_type")]
    int OutputType,
    [property: JsonPropertyName("file_type")]
    string FileType,
    [property: JsonPropertyName("order_by")]
    string OrderBy,
    [property: JsonPropertyName("sort_order")]
    string SortOrder,
    int Count,
    int Offset,
    int Limit,
    IReadOnlyList<DataPoint> Observations
);

/// <summary>
/// Represents an individual observation (data point) within the FRED response.
/// </summary>
public record DataPoint(
    [property: JsonPropertyName("realtime_start")]
    string RealtimeStart,
    [property: JsonPropertyName("realtime_end")]
    string RealtimeEnd,
    string Date,
    string Value
);