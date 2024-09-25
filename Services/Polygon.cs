// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Net.Http;
// using System.Text.Json;
// using Microsoft.AspNetCore.Http.HttpResults;

// namespace TradeLens.Services
// {
//     public class Polygon
//     {
//         private static HttpClient _httpClient = new()
//         {
//             BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
//         };

//         static async Task GetBars(HttpClient _httpClient)
//         {
//             using HttpResponseMessage response = await _httpClient.GetAsync("https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/2023-01-09/2023-02-10?adjusted=true&sort=asc&apiKey=UQE0AfiLy65mtQuTOX9mpQZqWV_Qb8iP");
//         }
//     }
// }
