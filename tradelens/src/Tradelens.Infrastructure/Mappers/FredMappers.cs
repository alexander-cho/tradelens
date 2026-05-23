using Tradelens.Core.Models;
using Tradelens.Infrastructure.Clients.Fred.DTOs;

namespace Tradelens.Infrastructure.Mappers;

public static class FredMappers
{
    
}

public static class SeriesObservationsMapper
{
    public static SeriesObservations ToSeriesObservations(SeriesDataDto seriesDataDto,
        SeriesObservationsDto seriesObservationsDto)
    {
        return new SeriesObservations
        {
            Id = seriesDataDto.Id,
            Title = seriesDataDto.Title,
            Frequency = seriesDataDto.Frequency,
            Units = seriesDataDto.Units,
            Observations = seriesObservationsDto.Observations.Select(DataPointMapper.ToDataPointModel).ToList()
        };
    }
}

internal static class DataPointMapper
{
    internal static DataPoint ToDataPointModel(DataPointDto dataPointDto)
    {
        return new DataPoint
        {
            Date = dataPointDto.Date,
            Value = ParseValue(dataPointDto.Value),
            RealtimeStart = dataPointDto.RealtimeStart,
            RealtimeEnd = dataPointDto.RealtimeEnd
        };
    }

    private static decimal? ParseValue(string value)
    {
        // FRED returns "." for missing values
        if (string.IsNullOrWhiteSpace(value) || value == ".")
        {
            return null;
        }

        return decimal.TryParse(value, out var result) ? result : null;
    }
}