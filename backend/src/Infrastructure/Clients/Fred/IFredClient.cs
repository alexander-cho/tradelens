using Infrastructure.Clients.Fred.DTOs;

namespace Infrastructure.Clients.Fred;

public interface IFredClient
{
    Task<SeriesDto?> GetSeriesAsync(string seriesId);
    Task<SeriesObservationsDto?> GetSeriesObservationsAsync(string seriesId);
}