using Microsoft.AspNetCore.Mvc;
using TradeLensAPI.Services;

namespace TradeLensAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AlphaVantageController : ControllerBase
    {
        private readonly AlphaVantageService _alphaVantageService;

        public AlphaVantageController(AlphaVantageService alphaVantageService)
        {
            _alphaVantageService = alphaVantageService;
        }
        
        // GET: api/v1/AlphaVantage/TopGainersLosersMostActive
        [HttpGet("TopGainersLosersMostActive")]
        public async Task<ActionResult> GetTopLosersGainers()
        {
            try
            {
                string data = await _alphaVantageService.GetTopGainersLosersMostActiveAsync();
                return Content(data, "application/json");
            } catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }
    }
}
