using Tradelens.Core.Interfaces;
using Tradelens.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;

[Route("api/market-data")]
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
    public async Task<ActionResult<MarketStatusModel>> GetMarketStatus()
    {
        return Ok(await this._marketDataService.GetMarketStatusAsync());
    }

    [HttpGet("earnings-calendar")]
    [Cache(1000)]
    public async Task<ActionResult<EarningsCalendarModel>> GetEarningsCalendar([FromQuery] string from, string to)
    {
        return Ok(await this._marketDataService.GetEarningsCalendarAsync(from, to));
    }
}