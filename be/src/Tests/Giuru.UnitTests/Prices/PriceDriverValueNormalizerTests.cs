using Buyer.Web.Shared.Services.Prices;

namespace Giuru.UnitTests.Prices
{
    public class PriceDriverValueNormalizerTests
    {
        [Theory]
        [InlineData("Tak", "Yes")]
        [InlineData(" TAK ", "Yes")]
        [InlineData("Yes", "Yes")]
        [InlineData("Ja", "Yes")]
        [InlineData("Nie", "No")]
        [InlineData("No", "No")]
        [InlineData(null, "No")]
        public void NormalizeExtraPacking_ReturnsCanonicalGrulaBooleanValue(string value, string expected)
        {
            Assert.Equal(expected, PriceDriverValueNormalizer.NormalizeExtraPacking(value));
        }
    }
}
