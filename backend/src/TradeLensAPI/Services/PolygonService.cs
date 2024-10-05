using DotNetEnv;


namespace TradeLensAPI.Services
{
    public class PolygonService
    {
        public static readonly HttpClient _httpClient;
        public string ticker = "SOFI";
        public int multiplier = 1;
        public string timespan = "day";
        public string startDate = "2023-11-02";
        public string endDate = "2024-09-25";

        static PolygonService()
        {
            Env.Load();
            _httpClient = new HttpClient();
        }

        public async Task<string> GetBarAggregatesDataAsync()
        {
            var POLYGON_API_KEY = Environment.GetEnvironmentVariable("POLYGON_API_KEY");
            Uri BaseAddress = new Uri($"https://api.polygon.io/v2/aggs/ticker/{this.ticker}/range/{this.multiplier}/{this.timespan}/{this.startDate}/{this.endDate}?adjusted=true&sort=asc&apiKey={POLYGON_API_KEY}");
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
