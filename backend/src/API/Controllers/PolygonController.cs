using Core.DTOs.Polygon;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PolygonController : ControllerBase
{
    private readonly IPolygonService _service;
    
    public PolygonController(IPolygonService service)
    {
        this._service = service;
    }

    // GET: api/Polygon/BarAggs
    [HttpGet("bar-aggs")]
    public async Task<ActionResult<BarAggregateDto>> GetBarAggregates([FromQuery] PolygonBarAggSpecParams polygonBarAggSpecParams)
    {
        return await this._service.GetBarAggregatesAsync(polygonBarAggSpecParams);
    }

    [HttpGet("related-companies")]
    public async Task<ActionResult<RelatedCompaniesDto>> GetRelatedCompanies([FromQuery] string ticker)
    {
        return await this._service.GetRelatedCompaniesAsync(ticker);
    }
}
