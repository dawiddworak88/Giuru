using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    /// <summary>The one Client.Api lookup that both the buyer and the seller app use.</summary>
    public class DiscountCodeLookupTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static ClientApiDiscountCodeLookup CreateLookup(string clientUrl)
        {
            return new ClientApiDiscountCodeLookup(new ApiClientService(), new TestPricingSettings { ClientUrl = clientUrl });
        }

        [Fact]
        public async Task ValidateAsync_AsksTheValidationEndpointForTheClientWithTheCallersTokenAndCulture_AndMapsAValidAnswer()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"Valid\",\"discountCode\":\"SUMMER25\"}"));
            var previous = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentUICulture = new CultureInfo("pl");

                var validation = await CreateLookup(api.Url).ValidateAsync(ClientId, "summer25", "token");

                Assert.True(validation.IsValid);
                Assert.Equal("SUMMER25", validation.DiscountCode);
            }
            finally
            {
                CultureInfo.CurrentUICulture = previous;
            }

            var request = Assert.Single(api.Requests);
            Assert.Equal("GET", request.Method);
            Assert.Equal("/api/v1/discountcodes/validation", request.Path);
            Assert.Equal("summer25", request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
            Assert.Equal("Bearer token", request.Authorization);
            Assert.Equal("pl", request.AcceptLanguage);
        }

        [Fact]
        public async Task ValidateAsync_SendsACodeWithSpecialCharactersEncodedSoItCannotChangeTheQuery()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, "{\"status\":\"NotFound\"}"));
            const string code = "A&ClientId=00000000-0000-0000-0000-000000000000 +/ü#?%20";

            await CreateLookup(api.Url).ValidateAsync(ClientId, code, "token");

            var request = Assert.Single(api.Requests);
            Assert.Equal(code, request.Query["Code"].ToString());
            Assert.Equal(ClientId.ToString(), request.Query["ClientId"].ToString());
            Assert.DoesNotContain(request.Query.Keys, key => key == "A");
        }

        [Theory]
        [InlineData("NotApplicable", DiscountCodeValidationStatus.NotApplicable)]
        [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
        [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
        [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
        [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData("Surprise", DiscountCodeValidationStatus.Unavailable)]
        [InlineData("0", DiscountCodeValidationStatus.Unavailable)]
        public async Task ValidateAsync_MapsTheStatusStrictly(string status, DiscountCodeValidationStatus expected)
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, $"{{\"status\":\"{status}\",\"discountCode\":\"SUMMER25\"}}"));

            var validation = await CreateLookup(api.Url).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(expected, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheBodyIsMissing_IsUnavailableNotAnAnswer()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (200, null));

            var validation = await CreateLookup(api.Url).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task ValidateAsync_WhenTheApiFails_ThrowsSoThatItIsNeverReadAsAnAnswer(HttpStatusCode statusCode)
        {
            await using var api = await FakeClientApi.StartAsync(_ => ((int)statusCode, "{\"message\":\"nope\"}"));

            var exception = await Assert.ThrowsAsync<CustomException>(() => CreateLookup(api.Url).ValidateAsync(ClientId, "SUMMER25", "token"));

            Assert.Equal((int)statusCode, exception.StatusCode);
        }

        [Fact]
        public async Task TheLookupAndTheValidator_TurnAFailedCallIntoAnUnverifiedCode()
        {
            await using var api = await FakeClientApi.StartAsync(_ => (500, "{\"message\":\"boom\"}"));
            var validator = new DiscountCodeValidator(CreateLookup(api.Url), Substitute.For<ILogger<DiscountCodeValidator>>());

            var validation = await validator.ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }
    }
}
