using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PolygonController : ControllerBase
{
    private readonly PolygonService _polygonService;
    
    public PolygonController(PolygonService polygonService)
    {
        this._polygonService = polygonService;
    }

    // GET: api/Polygon/BarAggs
    [HttpGet("BarAggs")]
    public async Task<ActionResult<string>> GetBarAggregates()
    {
        return await this._polygonService.GetBarAggregatesAsync();
    }

    [HttpGet("RelatedCompanies")]
    public async Task<ActionResult<string>> GetRelatedCompanies()
    {
        return await this._polygonService.GetRelatedCompaniesAsync();
    }
}
