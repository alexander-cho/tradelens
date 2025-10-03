using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OptionsController : ControllerBase
{
    private readonly IMaxPainService _maxPainService;
    private readonly IOptionsService _optionsService;
    
    public OptionsController(IMaxPainService maxPainService, IOptionsService optionsService)
    {
        this._maxPainService = maxPainService;
        this._optionsService = optionsService;
    }
    //
    // [HttpGet("cash-values")]
    // public CallsAndPutsCashSums GetCashValuesAndMaxPain([FromQuery] TradierOptionChainSpecParams tradierOptionChainSpecParams)
    // {
    //     return _maxPainService.CalculateCashValuesForOneExpirationAsync(tradierOptionChainSpecParams);
    // }

    [HttpGet("expirations")]
    public async Task<ActionResult<string>> GetExpiryListForUnderlying([FromQuery] string symbol)
    {
        return await _optionsService.GetExpiryListForUnderlyingAsync(symbol);
    }
}