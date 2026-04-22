using System.Text.Json;

namespace ST10330209PROG7311POE1.Patterns.Strategy
{
    public class LiveExchangeRateStrategy : IExchangeRateStrategy
    {
        private readonly HttpClient _httpClient;

        public LiveExchangeRateStrategy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                string url = "https://v6.exchange-rate-api.com/v6/19846ca05c5e51e56569fe57/latest/USD";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    decimal zarRate = doc.RootElement.GetProperty("rates").GetProperty("ZAR").GetDecimal();
                    return zarRate;
                }
                return 19.50m;
            }
            catch
            {
                return 19.50m;
            }
        }
    }
}