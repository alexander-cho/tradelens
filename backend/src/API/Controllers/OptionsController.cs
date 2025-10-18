using API.RequestHelpers;
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

    [Cache(600)]
    [HttpGet("options-chain")]
    public async Task<ActionResult<OptionsChainModel>> GetOptionsChain([FromQuery] TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        return await _optionsService.GetOptionsChainAsync(tradierOptionChainSpecParams);
    }
    
    [HttpGet("cash-values")]
    public async Task<ActionResult<CallsAndPutsCashSums>> GetCashValuesAndMaxPain([FromQuery] TradierOptionChainSpecParams tradierOptionChainSpecParams)
    {
        return await _optionsService.CalculateCashValuesForOneExpirationAsync(tradierOptionChainSpecParams);
    }

    [HttpGet("expirations")]
    public async Task<ActionResult<ExpirationsModel>> GetExpiryListForUnderlying([FromQuery] string symbol)
    {
        return await _optionsService.GetExpiryListForUnderlyingAsync(symbol);
    }
}