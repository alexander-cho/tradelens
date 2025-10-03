using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OptionsController : ControllerBase
{
    private readonly IOptionsService _optionsService;

    public OptionsController(IOptionsService optionsService)
    {
        this._optionsService = optionsService;
    }
    
    [HttpGet("cash-values")]
    public CallsAndPutsCashSums GetCashValuesAndMaxPain([FromQuery] TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        return _optionsService.CalculateCashValuesForOneExpirationAsync(tradierOptionChainSpecParams);
    }

    [HttpGet("expirations")]
    public async Task<ActionResult<ExpirationsModel>> GetExpiryListForUnderlying([FromQuery] string symbol)
    {
        return await _optionsService.GetExpiryListForUnderlyingAsync(symbol);
    }
}