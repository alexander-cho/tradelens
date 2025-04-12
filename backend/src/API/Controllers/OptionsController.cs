using Core.DTOs.Polygon;
using Core.DTOs.Tradier;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OptionsController : ControllerBase
{
    private readonly IMaxPainService _maxPainService;
    // private readonly IPolygonService _polygonService;
    
    public OptionsController(IMaxPainService service)
    {
        this._maxPainService = service;
    }

    [HttpGet("MaxPain")]
    public async Task<ActionResult<OptionsData>> GetMaxPain()
    {
        return await _maxPainService.GetMaxPainCalculation();
    }

    // [HttpGet("BarAggs")]
    // public async Task<ActionResult<BarAggregateDto>> GetBarAggregates()
    // {
    //     return await 
    // }
}