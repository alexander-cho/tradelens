using API.RequestHelpers;
using Core.DTOs.Fmp;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FmpController : ControllerBase
{
    private readonly IFmpService _service;

    public FmpController(IFmpService service)
    {
        this._service = service;
    }
    
    [Cache(600)]
    [HttpGet("House")]
    public async Task<ActionResult<IEnumerable<CongressTradesDto>>> GetHouseTrades()
    {
        return Ok(await this._service.GetLatestHouseTradesAsync());
    }

    [Cache(600)]
    [HttpGet("Senate")]
    public async Task<ActionResult<IEnumerable<CongressTradesDto>>> GetSenateTrades()
    {
        return Ok(await this._service.GetLatestSenateTradesAsync());
    }
}