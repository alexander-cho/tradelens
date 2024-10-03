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
//     public class AlphaVantageController : ControllerBase
//     {
//         public static readonly HttpClient _httpClient;

//         static AlphaVantageController()
//         {
//             Env.Load();
//             _httpClient = new HttpClient();
//         }
        
//         // GET: api/v1/AlphaVantage/TopGainersLosersMostActive
//         [HttpGet("TopGainersLosersMostActive")]
//         public async Task<ActionResult> GetTopLosersGainers(HttpClient _httpClient)
//         {
//             var ALPHAVANTAGE_API_KEY = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY");
//             Uri BaseAddress = new Uri($"https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={ALPHAVANTAGE_API_KEY}");
//             using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
//             response.EnsureSuccessStatusCode();
//             var stringResponse = await response.Content.ReadAsStringAsync();
//             var jsonResponse = Content(stringResponse, "application/json");
//             return jsonResponse;
//         }
//     }
// }
