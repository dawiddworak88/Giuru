using Buyer.Web.Shared.Services.Prices;
using Foundation.Pricing.Configurations;
using Foundation.Pricing.DependencyInjection;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class DiscountCodeCompositionTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static ServiceProvider BuildBuyerProvider(IDiscountCodeLookup lookup, ClaimsPrincipal user = null, string storedDiscountCode = null)
        {
            var storedDiscountCodeReader = Substitute.For<IStoredDiscountCodeReader>();
            storedDiscountCodeReader.ReadAsync(Arg.Any<Guid?>(), Arg.Any<string>()).Returns(Task.FromResult(storedDiscountCode));

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = user ?? new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants.ClientIdClaimType, ClientId.ToString())
                }, "test"))
            });

            var services = new ServiceCollection();
            services.AddSingleton(httpContextAccessor);
            services.AddSingleton(Substitute.For<ILogger<DiscountCodeValidator>>());
            services.AddSingleton(Substitute.For<ILogger<VerifiedDiscountCodePriceClientResolver>>());
            services.AddSingleton(lookup);
            services.AddSingleton(storedDiscountCodeReader);
            services.AddSingleton<IPricingSettings>(new TestPricingSettings());
            services.AddScoped<IDiscountCodeValidator, DiscountCodeValidator>();
            services.RegisterPriceClientResolvers<ClaimsPriceClientResolver>();

            // The container itself refuses a singleton that captures a scoped service, or a missing registration.
            return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
        }

        [Fact]
        public async Task TheProductResolver_IsTheValidatingDecorator_AndLeavesAnInapplicableCodeOut()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned)));

            using var provider = BuildBuyerProvider(lookup);
            using var scope = provider.CreateScope();

            var resolver = scope.ServiceProvider.GetRequiredService<IPriceClientResolver>();
            var priceClient = await resolver.ResolveAsync(null, "SUMMER25", "token");

            Assert.IsType<VerifiedDiscountCodePriceClientResolver>(resolver);
            Assert.Null(priceClient.DiscountCode);
            Assert.Equal(ClientId, priceClient.Id);
            await lookup.Received(1).ValidateAsync(ClientId, "SUMMER25", "token");
        }

        [Fact]
        public async Task TheBasketResolver_KeepsTheCodeItIsGivenWithoutAnyLookup()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();

            using var provider = BuildBuyerProvider(lookup);
            using var scope = provider.CreateScope();

            var resolver = scope.ServiceProvider.GetRequiredService<IBasketPriceClientResolver>();
            var priceClient = await resolver.ResolveAsync(null, "STORED25", "token");

            Assert.IsType<ClaimsPriceClientResolver>(resolver);
            Assert.Equal("STORED25", priceClient.DiscountCode);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task ARevokedStoredCode_IsPricedTheSameOnAProductPageAsInTheBasket()
        {
            // The product page must not show the undiscounted price while the basket charges the discounted one.
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled)));

            using var provider = BuildBuyerProvider(lookup, storedDiscountCode: "STORED25");
            using var scope = provider.CreateScope();

            var productPriceClient = await scope.ServiceProvider.GetRequiredService<IPriceClientResolver>().ResolveAsync(null, "STORED25", "token");
            var basketPriceClient = await scope.ServiceProvider.GetRequiredService<IBasketPriceClientResolver>().ResolveAsync(null, "STORED25", "token");

            Assert.Equal("STORED25", productPriceClient.DiscountCode);
            Assert.Equal(basketPriceClient.DiscountCode, productPriceClient.DiscountCode);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public void TheTwoResolvers_AreDifferentServices_AndTheBasketOneIsTheConcreteResolver()
        {
            using var provider = BuildBuyerProvider(Substitute.For<IDiscountCodeLookup>());
            using var scope = provider.CreateScope();

            var productResolver = scope.ServiceProvider.GetRequiredService<IPriceClientResolver>();
            var basketResolver = scope.ServiceProvider.GetRequiredService<IBasketPriceClientResolver>();
            var concrete = scope.ServiceProvider.GetRequiredService<ClaimsPriceClientResolver>();

            Assert.NotSame(productResolver, basketResolver);
            Assert.Same(concrete, basketResolver);
        }

        [Fact]
        public void BothAppsConcreteResolvers_AreBasketResolvers_SoTheyKeepTheCodeTheCoordinatorResolved()
        {
            Assert.True(typeof(IBasketPriceClientResolver).IsAssignableFrom(typeof(ClaimsPriceClientResolver)));
            Assert.True(typeof(IBasketPriceClientResolver).IsAssignableFrom(typeof(Seller.Web.Shared.Services.Prices.ClientRepositoryPriceClientResolver)));
            Assert.False(typeof(IBasketPriceClientResolver).IsAssignableFrom(typeof(VerifiedDiscountCodePriceClientResolver)));
        }

        [Fact]
        public void TheValidatorIsScoped_SoEachRequestStartsWithoutAnyRememberedAnswer()
        {
            using var provider = BuildBuyerProvider(Substitute.For<IDiscountCodeLookup>());

            IDiscountCodeValidator first;
            IDiscountCodeValidator second;

            using (var scope = provider.CreateScope())
            {
                first = scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();
                Assert.Same(first, scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>());
            }

            using (var scope = provider.CreateScope())
            {
                second = scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();
            }

            Assert.NotSame(first, second);
        }

        [Fact]
        public async Task AFreshPriceReadAndABasketSave_ShareOneLookupWithinTheRequest()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            using var provider = BuildBuyerProvider(lookup);
            using var scope = provider.CreateScope();

            var validator = scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();
            await validator.ValidateAsync(ClientId, "summer25", "token");
            await scope.ServiceProvider.GetRequiredService<IPriceClientResolver>().ResolveAsync(null, "SUMMER25", "token");

            await lookup.Received(1).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
