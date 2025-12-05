using API.RequestHelpers;
using Core.Models;
using Core.Specifications;
using Infrastructure.Clients.Polygon;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/bar-aggregates")]
[ApiController]
public class BarAggregatesController : ControllerBase
{
    private readonly IPolygonClient _polygonClient;

    public BarAggregatesController(IPolygonClient polygonClient)
    {
        _polygonClient = polygonClient;
    }
    
    [HttpGet]
    [Cache(1000)]
    public async Task<ActionResult<BarAggregatesModel>> GetBarAggregates([FromQuery] PolygonBarAggSpecParams polygonBarAggSpecParams)
    {
        var barAggregatesDto = await this._polygonClient.GetBarAggregatesAsync(polygonBarAggSpecParams);
        if (barAggregatesDto == null)
        {
            throw new InvalidOperationException("Could not fetch bar aggregates");
        }
        var barAggregates = BarAggregatesMapper.ToBarAggregateDomainModel(barAggregatesDto);
        
        return barAggregates;
    }
}