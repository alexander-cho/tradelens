using Core.Models;

namespace Core.Interfaces;

public interface IMacroService
{
    Task<SeriesObservations> GetSeriesObservationsDataAsync(string seriesId);
}