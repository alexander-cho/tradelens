using Tradelens.Domain.Interfaces;
using Tradelens.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MacroController : ControllerBase
{
    private readonly IMacroService _macroService;

    public MacroController(IMacroService macroService)
    {
        _macroService = macroService;
    }

    [Cache(10000)]
    [HttpGet("series-observations")]
    public async Task<ActionResult<SeriesObservations>> GetSeriesObservations([FromQuery] string seriesId)
    {
        return Ok(await _macroService.GetSeriesObservationsDataAsync(seriesId));
    }
}