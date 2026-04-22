using ST10330209PROG7311POE1.Patterns.Strategy;

namespace ST10330209PROG7311POE1.Services
{
    public class CurrencyService
    {
        private IExchangeRateStrategy _strategy;

        public CurrencyService(IExchangeRateStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IExchangeRateStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            
            return await _strategy.GetUsdToZarRateAsync();



        }

    }

}