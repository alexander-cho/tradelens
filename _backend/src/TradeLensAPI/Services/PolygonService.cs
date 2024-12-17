using DotNetEnv;


namespace TradeLensAPI.Services
{
    public class PolygonService
    {
        public static readonly HttpClient _httpClient;
        public string ticker = "SOFI";
        public int multiplier = 1;
        // minute, hour, day, week, month, quarter, year
        public string timespan = "hour";
        public string startDate = "2024-10-01";
        public string endDate = "2024-10-22";

        static PolygonService()
        {
            Env.Load();
            _httpClient = new HttpClient();
        }

        public async Task<string> GetBarAggregatesDataAsync()
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            Uri BaseAddress = new Uri($"https://api.polygon.io/v2/aggs/ticker/{this.ticker}/range/{this.multiplier}/{this.timespan}/{this.startDate}/{this.endDate}?adjusted=true&sort=asc&limit=50000&apiKey={POLYGON_API_KEY}");
            using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return stringResponse;
        }

        public async Task<string> GetRelatedCompaniesAsync()
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            Uri BaseAddress = new Uri($"https://api.polygon.io/v1/related-companies/{this.ticker}?apiKey={POLYGON_API_KEY}");
            using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return stringResponse;
        }
    }
}
