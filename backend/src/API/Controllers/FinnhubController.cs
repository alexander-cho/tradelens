using API.RequestHelpers;
using Core.DTOs.Finnhub;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinnhubController : ControllerBase
{
    private readonly IFinnhubService _service;

    public FinnhubController(IFinnhubService service)
    {
        _service = service;
    }
    
    [HttpGet("market-status")]
    [Cache(1000)]
    public async Task<ActionResult<MarketStatusDto>> GetMarketStatus()
    {
        return Ok(await this._service.GetMarketStatusAsync());
    }
}