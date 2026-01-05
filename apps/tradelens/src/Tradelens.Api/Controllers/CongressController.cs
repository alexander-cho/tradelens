using Tradelens.Core.Interfaces;
using Tradelens.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CongressController : ControllerBase
{
    private readonly ICongressService _congressService;

    public CongressController(ICongressService congressService)
    {
        this._congressService = congressService;
    }

    [Cache(600)]
    [HttpGet("trades")]
    public async Task<ActionResult<CongressTradeModel>> GetCongressTrades([FromQuery] string chamber)
    {
        var result = await _congressService.GetCongressTradesAsync(chamber);
        return Ok(result);
    }
}