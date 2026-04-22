using Xunit;

namespace ST10330209PROG7311POE1.Tests.Tests
{
    public class CurrencyCalculationTests
    {
        [Theory]
        [InlineData(100, 19.50, 1950)]
        [InlineData(50.25, 19.50, 979.875)]
        [InlineData(0, 19.50, 0)]
        [InlineData(1, 19.50, 19.50)]
        [InlineData(999.99, 19.50, 19499.805)]
        public void ConvertUsdToZar_ReturnsCorrectZar(decimal usd, decimal rate, decimal expected)
        {
            
            decimal result = usd * rate;

            
            Assert.Equal(expected, result);
        }
    }
}