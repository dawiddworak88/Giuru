using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Orders.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;

namespace Giuru.UnitTests.Services.Prices
{
    // The buyer-side check used by price lookups and the pages showing the Applied Discount Code:
    // the Client is always the one in the signed-in user's claims, and browsing never breaks on it.
    public class ApplicableDiscountCodeServiceTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        [Fact]
        public async Task GetApplicableAsync_WhenTheCodeIsValidForTheSignedInClient_ReturnsItsStoredSpelling()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            eligibility.GetValidDiscountCodeAsync("token", Arg.Any<string>(), ClientId, "summer25").Returns(Task.FromResult("SUMMER25"));
            var service = CreateService(eligibility, true, ClientId);

            Assert.Equal("SUMMER25", await service.GetApplicableAsync("token", "summer25"));
        }

        [Fact]
        public async Task GetApplicableAsync_WhenGrulaIsNotConfigured_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, false, ClientId);

            Assert.Null(await service.GetApplicableAsync("token", "SUMMER25"));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetApplicableAsync_WhenNoClientIsSignedIn_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, true, null);

            Assert.Null(await service.GetApplicableAsync("token", "SUMMER25"));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetApplicableAsync_WhenThereIsNoCode_ReturnsNullWithoutCallingClientApi()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var service = CreateService(eligibility, true, ClientId);

            Assert.Null(await service.GetApplicableAsync("token", "  "));
            await eligibility.DidNotReceive().GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetApplicableAsync_WhenClientApiAnswersWithAnError_ReturnsNullRatherThanFailing()
        {
            var service = CreateService(DiscountCodeEligibilityStubs.UnreachableRepository(), true, ClientId);

            Assert.Null(await service.GetApplicableAsync("token", "SUMMER25"));
        }

        [Fact]
        public async Task GetApplicableAsync_WhenClientApiCannotBeReached_ReturnsNullRatherThanFailing()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            eligibility.GetValidDiscountCodeAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>())
                .ThrowsAsync(new HttpRequestException("Connection refused"));
            var service = CreateService(eligibility, true, ClientId);

            Assert.Null(await service.GetApplicableAsync("token", "SUMMER25"));
        }

        private static ApplicableDiscountCodeService CreateService(IDiscountCodeEligibilityRepository eligibility, bool grulaConfigured, Guid? clientId)
        {
            var claims = clientId.HasValue
                ? new[] { new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, clientId.Value.ToString()) }
                : Array.Empty<Claim>();
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test")) });
            var options = Options.Create(new AppSettings
            {
                GrulaAccessToken = grulaConfigured ? "test-token" : null,
                GrulaEnvironmentId = grulaConfigured ? Guid.NewGuid().ToString() : null
            });

            return new ApplicableDiscountCodeService(httpContextAccessor, eligibility, options, Substitute.For<ILogger<ApplicableDiscountCodeService>>());
        }
    }
}
