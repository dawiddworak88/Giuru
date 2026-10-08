using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerBasket = Seller.Web.Areas.Orders.DomainModels.Basket;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using SellerBasketsApiController = Seller.Web.Areas.Orders.ApiControllers.BasketsApiController;
using SellerIBasketRepository = Seller.Web.Areas.Orders.Repositories.Baskets.IBasketRepository;
using SellerIProductColorsService = Seller.Web.Shared.Services.ProductColors.IProductColorsService;
using SellerIProductsRepository = Seller.Web.Areas.Shared.Repositories.Products.IProductsRepository;
using SellerIProductsService = Seller.Web.Shared.Services.Products.IProductsService;
using SellerPriceProductFactory = Seller.Web.Shared.Services.Prices.PriceProductFactory;
using SellerProduct = Seller.Web.Areas.Products.DomainModels.Product;
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;

namespace Giuru.UnitTests.Orders.Baskets
{
    // The user-facing side of the basket write seam: what the buyer and the seller are told, and what gets stored, when a
    // code is applied, dropped or cannot be verified. The coordinator's decisions are pinned in BasketDiscountCodeCoordinatorTests.
    public class BuyerBasketsApiControllerEnforcementTests
    {
        private static readonly Guid BasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid ProductId = Guid.NewGuid();

        private sealed class Fixture
        {
            public IBasketRepository BasketRepository { get; } = Substitute.For<IBasketRepository>();
            public IDiscountCodeValidator Validator { get; } = Substitute.For<IDiscountCodeValidator>();
            public BasketsApiController Controller { get; }

            public Fixture(string storedDiscountCode = null, bool enforcementEnabled = true)
            {
                var productsRepository = Substitute.For<IProductsRepository>();
                var priceService = Substitute.For<IPriceService>();
                var productsService = Substitute.For<IProductsService>();
                var productColorsService = Substitute.For<IProductColorsService>();
                var options = Options.Create(new AppSettings
                {
                    GrulaAccessToken = "test-token",
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    DiscountCodeEnforcementEnabled = enforcementEnabled
                });

                productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                    .Returns(Task.FromResult<IEnumerable<Product>>(new[] { new Product { Id = ProductId, Sku = "SKU", Name = "Product", PrimaryProductSku = "PRIMARY" } }));
                productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
                priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                    .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                    {
                        new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 12m, CurrencyCode = "EUR" } }
                    }));
                priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);

                BasketRepository.GetBasketById(Arg.Any<string>(), Arg.Any<string>(), BasketId)
                    .Returns(Task.FromResult(new BuyerBasket { Id = BasketId, DiscountCode = storedDiscountCode }));
                BasketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>(), Arg.Any<Guid?>())
                    .Returns(call => Task.FromResult(new BuyerBasket
                    {
                        Id = BasketId,
                        DiscountCode = call.ArgAt<string>(4),
                        DiscountCodeClientId = call.ArgAt<Guid?>(5),
                        Items = call.ArgAt<IEnumerable<BasketItem>>(3).ToList()
                    }));

                var httpContext = CreateHttpContext();
                var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
                httpContextAccessor.HttpContext.Returns(httpContext);

                Controller = new BasketsApiController(
                    BasketRepository,
                    Substitute.For<LinkGenerator>(),
                    Substitute.For<IMediaService>(),
                    TestLocalizer.Create<OrderResources>(),
                    productsRepository,
                    priceService,
                    productsService,
                    productColorsService,
                    options,
                    Substitute.For<ILogger<BasketsApiController>>(),
                    new PriceProductFactory(productsService, productColorsService, options),
                    new ClaimsPriceClientResolver(httpContextAccessor),
                    new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>()),
                    Validator)
                {
                    ControllerContext = new ControllerContext { HttpContext = httpContext }
                };
            }

            public void ValidatorAnswers(DiscountCodeValidation validation)
            {
                Validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(validation));
            }

            public Task<IActionResult> SaveAsync(string discountCode, bool includeDiscountCode = true)
            {
                var model = new SaveBasketRequestModel
                {
                    Items = new[] { new BasketItemRequestModel { ProductId = ProductId, Sku = "SKU", Name = "Product", Quantity = 1 } }
                };

                if (includeDiscountCode)
                {
                    model.DiscountCode = discountCode;
                }

                return Controller.Index(model);
            }

            public Task AssertNothingSavedAsync()
            {
                return BasketRepository.DidNotReceiveWithAnyArgs().SaveAsync(default, default, default, default, default, default);
            }
        }

        private static int StatusOf(IActionResult result) => ((ObjectResult)result).StatusCode.Value;

        private static string MessageOf(IActionResult result)
        {
            var value = ((ObjectResult)result).Value;

            return (string)value.GetType().GetProperty("Message").GetValue(value);
        }

        [Fact]
        public async Task Index_WithAValidNewCode_StoresTheCanonicalSpellingAndTheBuyersClient()
        {
            var fixture = new Fixture();
            fixture.ValidatorAnswers(DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.SaveAsync("summer25");

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            Assert.Equal("SUMMER25", ((BasketResponseModel)((ObjectResult)result).Value).DiscountCode);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<BasketItem>>(), "SUMMER25", ClientId);
            await fixture.Validator.Received().ValidateAsync(ClientId, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task Index_WithACodeThatCannotBeApplied_Returns400WithOneGenericMessageAndSavesNothing(DiscountCodeValidationStatus status)
        {
            var fixture = new Fixture();
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(status));

            var result = await fixture.SaveAsync("OTHER");

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));

            // A buyer is never told why: the same text for unknown, disabled and unassigned codes.
            Assert.Equal("DiscountCodeInvalid", MessageOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenTheCodeCannotBeVerified_Returns400WithTheCouldNotBeVerifiedMessageAndSavesNothing()
        {
            var fixture = new Fixture();
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var result = await fixture.SaveAsync("OTHER");

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));
            Assert.Equal("DiscountCodeCouldNotBeVerified", MessageOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsNoLongerApplicable_DropsItSavesTheBasketAndTellsTheBuyer()
        {
            var fixture = new Fixture(storedDiscountCode: "SUMMER25");
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            var result = await fixture.SaveAsync(null, includeDiscountCode: false);

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));

            var response = (BasketResponseModel)((ObjectResult)result).Value;
            Assert.Null(response.DiscountCode);
            Assert.Equal("DiscountCodeRemovedFromBasket", response.DiscountCodeRemovedMessage);

            // The basket is saved without the code and without the client it was verified for.
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Is<string>(x => x == null), Arg.Is<Guid?>(x => x == null));
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeCannotBeVerified_DoesNotDropIt()
        {
            var fixture = new Fixture(storedDiscountCode: "SUMMER25");
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var result = await fixture.SaveAsync(null, includeDiscountCode: false);

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WithAnExplicitRemoval_ValidatesNothingAndClearsTheClient()
        {
            var fixture = new Fixture(storedDiscountCode: "SUMMER25");

            var result = await fixture.SaveAsync(null);

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            Assert.Null(((BasketResponseModel)((ObjectResult)result).Value).DiscountCodeRemovedMessage);
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Is<string>(x => x == null), Arg.Is<Guid?>(x => x == null));
        }

        [Fact]
        public async Task Index_WithEnforcementOff_StoresTheTypedCodeWithoutAClientAndValidatesNothing()
        {
            var fixture = new Fixture(enforcementEnabled: false);

            var result = await fixture.SaveAsync("typed");

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<BasketItem>>(), "typed", Arg.Is<Guid?>(x => x == null));
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, ClientId.ToString()) }, "test");
            var context = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={BasketId}";

            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }
    }

    public class SellerBasketsApiControllerEnforcementTests
    {
        private static readonly Guid BasketId = Guid.NewGuid();
        private static readonly Guid ClientId = Guid.NewGuid();
        private static readonly Guid ProductId = Guid.NewGuid();

        private sealed class Fixture
        {
            public SellerIBasketRepository BasketRepository { get; } = Substitute.For<SellerIBasketRepository>();
            public IDiscountCodeValidator Validator { get; } = Substitute.For<IDiscountCodeValidator>();
            public SellerBasketsApiController Controller { get; }

            public Fixture(string storedDiscountCode = null, bool enforcementEnabled = true)
            {
                var productsRepository = Substitute.For<SellerIProductsRepository>();
                var priceService = Substitute.For<IPriceService>();
                var productsService = Substitute.For<SellerIProductsService>();
                var productColorsService = Substitute.For<SellerIProductColorsService>();
                var priceClientResolver = Substitute.For<IPriceClientResolver>();
                var options = Options.Create(new SellerAppSettings
                {
                    GrulaAccessToken = "test-token",
                    GrulaEnvironmentId = Guid.NewGuid().ToString(),
                    DiscountCodeEnforcementEnabled = enforcementEnabled
                });

                productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                    .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[] { new SellerProduct { Id = ProductId, Sku = "SKU", Name = "Product", PrimaryProductSku = "PRIMARY" } }));
                productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
                priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                    .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                    {
                        new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 12m, CurrencyCode = "EUR" } }
                    }));
                priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
                priceClientResolver.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0), DiscountCode = call.ArgAt<string>(1) }));

                BasketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), BasketId)
                    .Returns(Task.FromResult(new SellerBasket { Id = BasketId, DiscountCode = storedDiscountCode }));
                BasketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>(), Arg.Any<Guid?>())
                    .Returns(call => Task.FromResult(new SellerBasket
                    {
                        Id = BasketId,
                        DiscountCode = call.ArgAt<string>(4),
                        DiscountCodeClientId = call.ArgAt<Guid?>(5),
                        Items = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList()
                    }));

                Controller = new SellerBasketsApiController(
                    BasketRepository,
                    Substitute.For<LinkGenerator>(),
                    Substitute.For<IMediaService>(),
                    productsRepository,
                    priceService,
                    productsService,
                    productColorsService,
                    TestLocalizer.Create<OrderResources>(),
                    options,
                    priceClientResolver,
                    Substitute.For<ILogger<SellerBasketsApiController>>(),
                    new SellerPriceProductFactory(productsService, productColorsService, options),
                    new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>()),
                    Validator)
                {
                    ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
                };
            }

            public void ValidatorAnswers(DiscountCodeValidation validation)
            {
                Validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(validation));
            }

            public Task<IActionResult> SaveAsync(string discountCode, bool includeDiscountCode = true)
            {
                var model = new SellerSaveBasketRequestModel
                {
                    Id = BasketId,
                    ClientId = ClientId,
                    Items = new[] { new SellerBasketItemRequestModel { ProductId = ProductId, Sku = "SKU", Name = "Product", Quantity = 1 } }
                };

                if (includeDiscountCode)
                {
                    model.DiscountCode = discountCode;
                }

                return Controller.Index(model);
            }

            public Task AssertNothingSavedAsync()
            {
                return BasketRepository.DidNotReceiveWithAnyArgs().SaveAsync(default, default, default, default, default, default);
            }
        }

        private static int StatusOf(IActionResult result) => ((ObjectResult)result).StatusCode.Value;

        private static string MessageOf(IActionResult result)
        {
            var value = ((ObjectResult)result).Value;

            return (string)value.GetType().GetProperty("Message").GetValue(value);
        }

        [Fact]
        public async Task Index_WithAValidNewCode_StoresTheCanonicalSpellingAndTheClientTheSellerWorksFor()
        {
            var fixture = new Fixture();
            fixture.ValidatorAnswers(DiscountCodeValidation.Valid("SUMMER25"));

            var result = await fixture.SaveAsync("summer25");

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            Assert.Equal("SUMMER25", ((SellerBasketResponseModel)((ObjectResult)result).Value).DiscountCode);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<SellerBasketItem>>(), "SUMMER25", ClientId);
            await fixture.Validator.Received().ValidateAsync(ClientId, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.Disabled, "DiscountCodeDisabled")]
        [InlineData(DiscountCodeValidationStatus.NotAssigned, "DiscountCodeNotAssignedToClient")]
        [InlineData(DiscountCodeValidationStatus.NotFound, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.Unavailable, "DiscountCodeCouldNotBeVerified")]
        public async Task Index_WithACodeThatCannotBeApplied_Returns400WithTheSpecificReasonAndSavesNothing(DiscountCodeValidationStatus status, string expectedMessage)
        {
            var fixture = new Fixture();
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(status));

            var result = await fixture.SaveAsync("OTHER");

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));

            // Sellers can fix what is wrong, so they are told what it is.
            Assert.Equal(expectedMessage, MessageOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsNoLongerApplicable_DropsItSavesTheBasketAndTellsTheSeller()
        {
            var fixture = new Fixture(storedDiscountCode: "SUMMER25");
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            var result = await fixture.SaveAsync(null, includeDiscountCode: false);

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));

            var response = (SellerBasketResponseModel)((ObjectResult)result).Value;
            Assert.Null(response.DiscountCode);
            Assert.Equal("DiscountCodeRemovedFromBasket", response.DiscountCodeRemovedMessage);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Is<string>(x => x == null), Arg.Is<Guid?>(x => x == null));
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeCannotBeVerified_DoesNotDropIt()
        {
            var fixture = new Fixture(storedDiscountCode: "SUMMER25");
            fixture.ValidatorAnswers(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var result = await fixture.SaveAsync(null, includeDiscountCode: false);

            Assert.Equal((int)HttpStatusCode.BadRequest, StatusOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WithEnforcementOff_StoresTheTypedCodeWithoutAClientAndValidatesNothing()
        {
            var fixture = new Fixture(enforcementEnabled: false);

            var result = await fixture.SaveAsync("typed");

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            await fixture.Validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
            await fixture.BasketRepository.Received(1).SaveAsync(
                Arg.Any<string>(), Arg.Any<string>(), BasketId, Arg.Any<IEnumerable<SellerBasketItem>>(), "typed", Arg.Is<Guid?>(x => x == null));
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) };
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }
    }
}
