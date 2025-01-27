using Core.Interfaces;
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
    [HttpGet("BarAggs")]
    public async Task<ActionResult<string>> GetBarAggregates()
    {
        return await this._service.GetBarAggregatesAsync();
    }

    [HttpGet("RelatedCompanies")]
    public async Task<ActionResult<string>> GetRelatedCompanies()
    {
        return await this._service.GetRelatedCompaniesAsync();
    }
}
