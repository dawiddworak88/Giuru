using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Services.DiscountCodes;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using BuyerBasketRepository = Buyer.Web.Areas.Orders.Repositories.Baskets.IBasketRepository;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class StoredDiscountCodeReaderTests
    {
        private static readonly Guid BasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();

        private static IHttpContextAccessor Request(Action<HttpRequest> configure)
        {
            var httpContext = new DefaultHttpContext();
            configure(httpContext.Request);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            return httpContextAccessor;
        }

        private static IHttpContextAccessor WithBasketCookie(string basketCookie)
        {
            return Request(request => request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={basketCookie}");
        }

        [Fact]
        public async Task TheBuyerReader_ReadsTheCodeOfTheBasketTheCookieNames_OncePerRequest()
        {
            var basketRepository = Substitute.For<BuyerBasketRepository>();
            basketRepository.GetBasketById("token", Arg.Any<string>(), BasketId)
                .Returns(Task.FromResult(new BuyerBasket { Id = BasketId, DiscountCode = "SUMMER25" }));

            var accessor = WithBasketCookie(BasketId.ToString());
            DiscountCodeBindingTestHelper.AddProof(accessor.HttpContext, BasketId, ClientId, "SUMMER25");
            var reader = new BasketCookieDiscountCodeReader(accessor, basketRepository, DiscountCodeBindingTestHelper.Create(accessor.HttpContext));

            Assert.Equal("SUMMER25", await reader.ReadAsync(ClientId, "token"));
            Assert.Equal("SUMMER25", await reader.ReadAsync(ClientId, "token"));
            Assert.Null(await reader.ReadAsync(Guid.NewGuid(), "token"));
            await basketRepository.Received(1).GetBasketById(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>());
        }

        [Theory]
        [InlineData("")]
        [InlineData("not-a-guid")]
        public async Task TheBuyerReader_WithoutAUsableBasketCookie_HasNoStoredCodeAndReadsNothing(string basketCookie)
        {
            var basketRepository = Substitute.For<BuyerBasketRepository>();

            var accessor = WithBasketCookie(basketCookie);
            var reader = new BasketCookieDiscountCodeReader(accessor, basketRepository, DiscountCodeBindingTestHelper.Create(accessor.HttpContext));

            Assert.Null(await reader.ReadAsync(ClientId, "token"));
            await basketRepository.DidNotReceiveWithAnyArgs().GetBasketById(default, default, default);
        }

        [Fact]
        public async Task TheBuyerReader_WhenTheBasketDoesNotExist_HasNoStoredCode()
        {
            var basketRepository = Substitute.For<BuyerBasketRepository>();
            basketRepository.GetBasketById(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(Task.FromResult<BuyerBasket>(null));

            var accessor = WithBasketCookie(BasketId.ToString());
            var reader = new BasketCookieDiscountCodeReader(accessor, basketRepository, DiscountCodeBindingTestHelper.Create(accessor.HttpContext));

            Assert.Null(await reader.ReadAsync(ClientId, "token"));
        }

        [Theory]
        [InlineData("another-client")]
        [InlineData("another-basket")]
        [InlineData("another-code")]
        [InlineData("missing")]
        [InlineData("tampered")]
        [InlineData("unknown-client")]
        public async Task PriceRead_WithAnUntrustedBinding_ValidatesForTheCurrentClient(string scenario)
        {
            var accessor = WithBasketCookie(BasketId.ToString());
            var httpContext = accessor.HttpContext;
            var claims = scenario == "unknown-client" ? Array.Empty<Claim>() : new[] { new Claim("ClientId", ClientId.ToString()) };
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

            if (scenario == "tampered")
            {
                httpContext.Request.Headers.Cookie += $"; {BasketConstants.DiscountCodeBindingCookieName}=forged-proof";
            }
            else if (scenario != "missing")
            {
                // The cookie can be copied together with the basket cookie. Its client, basket and code must all match.
                DiscountCodeBindingTestHelper.AddProof(httpContext,
                    scenario == "another-basket" ? Guid.NewGuid() : BasketId,
                    scenario == "another-client" ? Guid.NewGuid() : ClientId,
                    scenario == "another-code" ? "OTHER25" : "SUMMER25");
            }

            var repository = Substitute.For<BuyerBasketRepository>();
            repository.GetBasketById("token", Arg.Any<string>(), BasketId)
                .Returns(Task.FromResult(new BuyerBasket { Id = BasketId, DiscountCode = "SUMMER25" }));
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);
            var reader = new BasketCookieDiscountCodeReader(accessor, repository, DiscountCodeBindingTestHelper.Create(httpContext));
            var resolver = new VerifiedDiscountCodePriceClientResolver(new ClaimsPriceClientResolver(accessor), validator,
                reader, new TestPricingSettings(), NullLogger<VerifiedDiscountCodePriceClientResolver>.Instance);

            var priceClient = await resolver.ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.Received(1).ValidateAsync(scenario == "unknown-client" ? null : ClientId, "SUMMER25", "token");
        }

        [Fact]
        public async Task PriceRead_ForTheOriginalClient_KeepsItsBoundCodeAfterRevocation()
        {
            var accessor = WithBasketCookie(BasketId.ToString());
            accessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("ClientId", ClientId.ToString()) }, "test"));
            DiscountCodeBindingTestHelper.AddProof(accessor.HttpContext, BasketId, ClientId, "SUMMER25");
            var repository = Substitute.For<BuyerBasketRepository>();
            repository.GetBasketById("token", Arg.Any<string>(), BasketId)
                .Returns(Task.FromResult(new BuyerBasket { Id = BasketId, DiscountCode = "SUMMER25" }));
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);
            var reader = new BasketCookieDiscountCodeReader(accessor, repository, DiscountCodeBindingTestHelper.Create(accessor.HttpContext));
            var resolver = new VerifiedDiscountCodePriceClientResolver(new ClaimsPriceClientResolver(accessor), validator,
                reader, new TestPricingSettings(), NullLogger<VerifiedDiscountCodePriceClientResolver>.Instance);

            var priceClient = await resolver.ResolveAsync(null, " summer25 ", "token");

            Assert.Equal("SUMMER25", priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }
}
