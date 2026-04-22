namespace ST10330209PROG7311POE1.Patterns.Strategy
{
    public class FallbackExchangeRateStrategy : IExchangeRateStrategy
    {
        public Task<decimal> GetUsdToZarRateAsync()
        {
            
            return Task.FromResult(19.50m);
        }
    }
}