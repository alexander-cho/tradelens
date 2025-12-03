using Core.Interfaces;
using Core.Models;
using Infrastructure.Clients.Fred;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public class MacroService : IMacroService
{
    private readonly IFredClient _fredClient;

    public MacroService(IFredClient fredClient)
    {
        _fredClient = fredClient;
    }
    
    // combine series/ and series/observations
    public async Task<SeriesObservations> GetSeriesObservationsDataAsync(string seriesId)
    {
        var seriesDto = await _fredClient.GetSeriesAsync(seriesId);
        var seriesDataDto = seriesDto?.Seriess.ElementAt(0);
        var seriesObservationsDto = await _fredClient.GetSeriesObservationsAsync(seriesId);
        
        if (seriesDataDto == null || seriesObservationsDto == null)
        {
            throw new InvalidOperationException("Market status data was not available");
        }

        var seriesObservations = SeriesObservationsMapper.ToSeriesObservations(seriesDataDto, seriesObservationsDto);

        return seriesObservations;
    }
}