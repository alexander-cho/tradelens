// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using System.Text.Json;

// using TradeLens.Services;

// namespace TradeLens.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class PolygonController: ControllerBase
//     {
//         private static readonly HttpClient _httpClient;

//         static PolygonController()
//         {
//             _httpClient = new HttpClient();
//         }

//         [HttpGet]
//         public async static Task<ActionResult<Polygon>> GetBars()
//         {
//             var result = new List<Polygon>();
//             string apiUrl = "https://api.polygon.io/v2/aggs/ticker/SOFI/range/1/day/2024-01-09/2024-09-10?adjusted=true&sort=asc&apiKey=UQE0AfiLy65mtQuTOX9mpQZqWV_Qb8iP";
//             HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
//             if (response.IsSuccessStatusCode)
//             {
//                 string stringResponse = await response.Content.ReadAsStringAsync();
//                 var polygonResponse = JsonSerializer.Deserialize<Polygon>(stringResponse, new JsonSerializerOptions()
//                 {
//                     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//                 });
//                 result.Add(polygonResponse);
//             }
//             else
//             {
//                 throw new Exception("Not found.");
//             }
//         }
//     }
// }