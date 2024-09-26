using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DotNetEnv;

namespace TradeLens.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PolygonController: ControllerBase
    {
        public static readonly HttpClient _httpClient;
        public string stocksTicker = "SOFI";
        public int multiplier = 1;
        public string timespan = "day";
        public string startDate = "2024-09-01";
        public string endDate = "2024-09-24";

        static PolygonController()
        {
            Env.Load();
            _httpClient = new HttpClient();
        }

        [HttpGet("Ticker")]
        public async Task GetTickerAggs(HttpClient _httpClient)
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            using HttpResponseMessage response = await _httpClient.GetAsync($"https://api.polygon.io/v2/aggs/ticker/{this.stocksTicker}/range/{this.multiplier}/{this.timespan}/{this.startDate}/{this.endDate}?adjusted=true&sort=asc&apiKey={POLYGON_API_KEY}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
            return;
        }

        [HttpGet("Options")]
        public async Task GetOptionAggs(HttpClient _httpClient)
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            using HttpResponseMessage response = await _httpClient.GetAsync($"https://api.polygon.io/v2/aggs/ticker/O:SOFI241004C00008000/range/1/day/2024-09-12/2024-09-24?adjusted=true&sort=asc&apiKey={POLYGON_API_KEY}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
            return;
        }

        [HttpGet("Crypto")]
        public async Task GetCryptoAggs(HttpClient _httpClient)
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            using HttpResponseMessage response = await _httpClient.GetAsync($"https://api.polygon.io/v2/aggs/ticker/X:BTCUSD/range/1/day/2024-01-09/2024-09-09?apiKey={POLYGON_API_KEY}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
            return;
        }
    }
}
