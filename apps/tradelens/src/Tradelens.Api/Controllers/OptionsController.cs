using Tradelens.Core.Interfaces;
using Tradelens.Core.Models;
using Tradelens.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;

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