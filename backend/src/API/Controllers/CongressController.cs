using API.RequestHelpers;
using Core.Interfaces;
using Infrastructure.Clients.Fmp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

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
    public async Task<ActionResult<string>> GetCongressTrades([FromQuery] string chamber)
    {
        var result = await _congressService.GetCongressTradesAsync(chamber);
        return Ok(result);
    }
}