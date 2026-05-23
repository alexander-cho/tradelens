using Tradelens.Core.Models;

namespace Tradelens.Core.Interfaces;

public interface IMacroService
{
    Task<SeriesObservations> GetSeriesObservationsDataAsync(string seriesId);
}