using Tradelens.Core.Models;
using Tradelens.Core.Specifications;
using Tradelens.Infrastructure.Clients.Polygon;
using Tradelens.Infrastructure.Mappers;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;

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
    public async Task<ActionResult<BarAggregatesModel>> GetBarAggregates([FromQuery] PolygonBarAggQueryParams polygonBarAggSpecParams)
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