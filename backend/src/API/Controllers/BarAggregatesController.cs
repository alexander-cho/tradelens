using Core.DTOs.Polygon;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BarAggregatesController : ControllerBase
{
    private readonly IPolygonClient _polygonClient;

    public BarAggregatesController(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }
    
    [HttpGet]
    public async Task<ActionResult<BarAggregateDto?>> GetBarAggregates([FromQuery] PolygonBarAggSpecParams polygonBarAggSpecParams)
    {
        return await this._polygonClient.GetBarAggregatesAsync(polygonBarAggSpecParams);
    }
}