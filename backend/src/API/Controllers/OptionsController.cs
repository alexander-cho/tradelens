using Core.DTOs.Tradier;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OptionsController : ControllerBase
{
    private readonly IMaxPainService _maxPainService;
    private readonly ITradierService _tradierService;
    // private readonly IPolygonService _polygonService;
    
    public OptionsController(IMaxPainService maxPainService, ITradierService tradierService)
    {
        this._maxPainService = maxPainService;
        this._tradierService = tradierService;
    }
    
    [HttpGet("MaxPain")]
    public async Task<ActionResult<OptionsData>> GetMaxPain([FromQuery] TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        return await _maxPainService.GetMaxPainCalculationAsync(tradierOptionChainSpecParams);
    }

    [HttpGet("expirations")]
    public async Task<ActionResult<ExpiryData>> GetExpiryListForUnderlying([FromQuery] string symbol)
    {
        return await _tradierService.GetExpiryListForUnderlyingAsync(symbol);
    }

    // [HttpGet("BarAggs")]
    // public async Task<ActionResult<BarAggregateDto>> GetBarAggregates()
    // {
    //     return await 
    // }
}