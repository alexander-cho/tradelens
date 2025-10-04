using API.RequestHelpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarketDataController : ControllerBase
{
    private readonly IMarketDataService _marketDataService;

    public MarketDataController(IMarketDataService marketDataService)
    {
        _marketDataService = marketDataService;
    }
    
    [HttpGet("market-status")]
    [Cache(1000)]
    public async Task<ActionResult<string>> GetMarketStatus()
    {
        return Ok(await this._marketDataService.GetMarketStatusAsync());
    }
}