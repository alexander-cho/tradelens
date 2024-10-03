// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using System.Text.Json;
// using DotNetEnv;

// namespace TradeLensApi.Controllers
// {
//     [Route("api/v1/[controller]")]
//     [ApiController]
//     public class PolygonController: ControllerBase
//     {
//         public static readonly HttpClient _httpClient;
//         public string ticker = "SOFI";
//         public int multiplier = 1;
//         public string timespan = "day";
//         public string startDate = "2024-09-02";
//         public string endDate = "2024-09-25";

//         static PolygonController()
//         {
//             Env.Load();
//             _httpClient = new HttpClient();
//         }

//         // GET: api/v1/Polygon/BarAggs
//         [HttpGet("BarAggs")]
//         public async Task<ActionResult> GetBarAggs(HttpClient _httpClient)
//         {
//             var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
//             Uri BaseAddress = new Uri($"https://api.polygon.io/v2/aggs/ticker/{this.ticker}/range/{this.multiplier}/{this.timespan}/{this.startDate}/{this.endDate}?adjusted=true&sort=asc&apiKey={POLYGON_API_KEY}");
//             using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
//             response.EnsureSuccessStatusCode();
//             var stringResponse = await response.Content.ReadAsStringAsync();
//             var jsonResponse = Content(stringResponse, "application/json");
//             return jsonResponse;
//         }

//         // GET: api/v1/Polygon/RelatedCompanies
//         [HttpGet("RelatedCompanies")]
//         public async Task<ActionResult> GetRelatedCompanies(HttpClient _httpClient)
//         {
//             var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
//             Uri BaseAddress = new Uri($"https://api.polygon.io/v1/related-companies/{this.ticker}?apiKey={POLYGON_API_KEY}");
//             using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
//             response.EnsureSuccessStatusCode();
//             var stringResponse = await response.Content.ReadAsStringAsync();
//             var jsonResponse = Content(stringResponse, "application/json");
//             return jsonResponse;
//         }
//     }
// }
