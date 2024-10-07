// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using System.Text.Json;
// using DotNetEnv;

// namespace TradeLensAPI.Controllers
// {
//     [Route("api/v1/[controller]")]
//     [ApiController]
//     public class TradierController : ControllerBase
//     {
//         public static readonly HttpClient _httpClient;
//         public string ticker = "SOFI";
//         public string expiryDate = "2024-10-04";

//         static TradierController()
//         {
//             Env.Load();
//             _httpClient = new HttpClient();
//         }

//         // GET: api/v1/Tradier/OptionChains
//         [HttpGet("OptionChains")]
//         public async Task<ActionResult> GetOptionChains(HttpClient _httpClient)
//         {
//             Console.WriteLine(this.ticker);
//             Console.WriteLine(this.expiryDate);
//             var TRADIER_API_KEY = Environment.GetEnvironmentVariable("TRADIER_API_KEY");
//             Console.WriteLine(TRADIER_API_KEY);
//             Uri BaseAddress = new Uri($"https://api.tradier.com/v1/markets/options/chains?symbol={this.ticker}&expiration={this.expiryDate}&greeks=true");
//             Console.WriteLine(BaseAddress);
//             _httpClient.DefaultRequestHeaders.Add("Authorization", TRADIER_API_KEY);
//             var request = await _httpClient.GetAsync(BaseAddress);
//             request.EnsureSuccessStatusCode();
//             var stringResponse = await request.Content.ReadAsStringAsync();
//             var jsonResponse = Content(stringResponse, "application/json");
//             Console.WriteLine(jsonResponse.StatusCode);
//             return jsonResponse;
//         }
//     }
// }
