using DotNetEnv;


namespace TradeLensAPI.Services
{
    public class AlphaVantageService
    {
        public static readonly HttpClient _httpClient;

        static AlphaVantageService()
        {
            Env.Load();
            _httpClient = new HttpClient();
        }

        public async Task<string> GetTopGainersLosersMostActiveAsync()
        {
            var ALPHAVANTAGE_API_KEY = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY");
            Uri BaseAddress = new Uri($"https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={ALPHAVANTAGE_API_KEY}");
            using HttpResponseMessage response = await _httpClient.GetAsync(BaseAddress);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return stringResponse;
        }
    }
}