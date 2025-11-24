using API.RequestHelpers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
    [HttpGet("margin-balance")]
    public async Task<ActionResult<MarginBalanceModel>> GetMarginBalance()
    {
        return Ok(await _macroService.GetMarginBalanceAsync());
    }
    
    [Cache(10000)]
    [HttpGet("money-market")]
    public async Task<ActionResult<MoneyMarketFundsModel>> GetMoneyMarketFunds()
    {
        return Ok(await _macroService.GetMoneyMarketFundsAsync());
    }
}