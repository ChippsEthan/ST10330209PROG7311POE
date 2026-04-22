namespace ST10330209PROG7311POE1.Patterns.Strategy
{
    public interface IExchangeRateStrategy
    {
        Task<decimal> GetUsdToZarRateAsync();
    }
}