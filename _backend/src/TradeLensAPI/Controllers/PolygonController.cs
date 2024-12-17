using Microsoft.AspNetCore.Mvc;

using TradeLensAPI.Services;


namespace TradeLensAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PolygonController : ControllerBase
    {
        private readonly PolygonService _polygonService;

        public PolygonController(PolygonService polygonService)
        {
            _polygonService = polygonService;
        }

        // GET: api/v1/Polygon/BarAggs
        [HttpGet("BarAggs")]
        public async Task<ActionResult> GetBarAggs()
        {
            try
            {
                string data = await _polygonService.GetBarAggregatesDataAsync();
                return Content(data, "application/json");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }

        // GET: api/v1/Polygon/RelatedCompanies
        [HttpGet("RelatedCompanies")]
        public async Task<ActionResult> GetRelatedCompanies()
        {
            try
            {
                string data = await _polygonService.GetRelatedCompaniesAsync();
                return Content(data, "application/json");
            } 
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }
    }
}
