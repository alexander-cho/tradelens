using Core.Specifications;
using Infrastructure.Clients.Polygon;
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
    //
    // [HttpGet]
    // public async Task<ActionResult<string>> GetBarAggregates([FromQuery] PolygonBarAggSpecParams polygonBarAggSpecParams)
    // {
    //     return await this._polygonClient.GetBarAggregatesAsync(polygonBarAggSpecParams);
    //     
    // }
}