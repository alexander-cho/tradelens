using Tradelens.Domain.Models;

namespace Tradelens.Domain.Interfaces;

public interface IMacroService
{
    Task<SeriesObservations> GetSeriesObservationsDataAsync(string seriesId);
}