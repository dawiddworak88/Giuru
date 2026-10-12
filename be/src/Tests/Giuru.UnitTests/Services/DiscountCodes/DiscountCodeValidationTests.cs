using Foundation.Pricing.DiscountCodes;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class DiscountCodeValidationTests
    {
        [Theory]
        [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
        [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
        [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
        [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData("NotApplicable", DiscountCodeValidationStatus.NotApplicable)]
        public void FromApiResponse_MapsEachSpecificStatusAndDropsTheCode(string status, DiscountCodeValidationStatus expected)
        {
            var validation = DiscountCodeValidation.FromApiResponse(status, "SUMMER25");

            Assert.Equal(expected, validation.Status);
            Assert.False(validation.IsValid);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public void FromApiResponse_MapsValidTogetherWithTheCanonicalCode()
        {
            var validation = DiscountCodeValidation.FromApiResponse("Valid", "SUMMER25");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("valid")]
        [InlineData("VALID")]
        [InlineData("Unavailable")]
        [InlineData("Surprise")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("42")]
        [InlineData(" Valid")]
        [InlineData("-1")]
        public void FromApiResponse_TreatsAMissingOrUnknownStatusAsUnavailable(string status)
        {
            var validation = DiscountCodeValidation.FromApiResponse(status, "SUMMER25");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            Assert.False(validation.IsValid);
            Assert.Null(validation.DiscountCode);
        }
    }
}
