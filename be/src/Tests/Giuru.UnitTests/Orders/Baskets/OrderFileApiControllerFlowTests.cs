using Buyer.Web.Areas.Orders.ApiControllers;
using Giuru.UnitTests.Helpers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using DomainBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Services.OrderFiles;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Buyer.Web.Shared.Repositories.Inventory;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Pricing.Services;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerBasket = Seller.Web.Areas.Orders.DomainModels.Basket;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerIBasketRepository = Seller.Web.Areas.Orders.Repositories.Baskets.IBasketRepository;
using SellerIInventoryRepository = Seller.Web.Shared.Repositories.Inventory.IInventoryRepository;
using SellerInventoryItem = Seller.Web.Shared.DomainModels.Inventory.InventoryItem;
using SellerIOrderFileService = Seller.Web.Areas.Orders.Services.OrderFiles.IOrderFileService;
using SellerIOrdersRepository = Seller.Web.Areas.Orders.Repositories.Orders.IOrdersRepository;
using SellerIMediaItemsRepository = Seller.Web.Areas.Shared.Repositories.Media.IMediaItemsRepository;
using SellerIProductColorsService = Seller.Web.Shared.Services.ProductColors.IProductColorsService;
using SellerIProductsRepository = Seller.Web.Areas.Shared.Repositories.Products.IProductsRepository;
using SellerIProductsService = Seller.Web.Shared.Services.Products.IProductsService;
using SellerOrderFileApiController = Seller.Web.Areas.Orders.ApiControllers.OrderFileApiController;
using SellerOrderFileLine = Seller.Web.Areas.Orders.DomainModels.OrderFileLine;
using SellerPriceProductFactory = Seller.Web.Shared.Services.Prices.PriceProductFactory;
using SellerProduct = Seller.Web.Areas.Products.DomainModels.Product;
using SellerUploadMediaRequestModel = Seller.Web.Areas.Media.ApiRequestModels.UploadMediaRequestModel;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class BuyerOrderFileApiControllerFlowTests
    {
        [Fact]
        public async Task Index_WhenMergedLineHasAnUnpricedPricingResult_DoesNotRetainTheExistingPrice()
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            IEnumerable<BasketItem> savedItems = null;
            var existingBasket = new DomainBasket
            {
                Id = basketId,
                Items = new[]
                {
                    new BasketItem
                    {
                        ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2,
                        ExternalReference = "reference", MoreInfo = "notes", UnitPrice = 10m, Price = 20m, Currency = "EUR"
                    }
                }
            };
            var orderFileService = Substitute.For<IOrderFileService>();
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var inventoryRepository = Substitute.For<IInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[]
            {
                new OrderFileLine { Sku = "SKU", Quantity = 3, ExternalReference = "reference", MoreInfo = "notes" }
            });
            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId).Returns(Task.FromResult(existingBasket));
            productsRepository.GetProductsBySkusAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[]
                {
                    new Product { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" }
                }));
            inventoryRepository.GetStockAvailbleProductsByProductIdsAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                .Returns(Task.FromResult<IEnumerable<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>>(Array.Empty<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>()));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.InvalidPriceDrivers }
                }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    savedItems = call.ArgAt<IEnumerable<BasketItem>>(3).ToList();
                    return Task.FromResult(new DomainBasket { Id = basketId, Items = savedItems });
                });

            var httpContext = CreateHttpContext(basketId);

            var controller = new OrderFileApiController(
                orderFileService,
                productsRepository,
                basketRepository,
                Substitute.For<LinkGenerator>(),
                options,
                Substitute.For<IMediaService>(),
                Substitute.For<IMediaItemsRepository>(),
                Substitute.For<IOrdersRepository>(),
                inventoryRepository,
                Substitute.For<ILogger<OrderFileApiController>>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                priceService,
                productsService,
                productColorsService,
                new PriceProductFactory(productsService, productColorsService, options),
                CreatePriceClientResolver(httpContext),
                CreateBasketRepricingService(priceService),
                Substitute.For<IDiscountCodeValidator>())
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.Index(new UploadMediaRequestModel { File = Substitute.For<IFormFile>() });

            Assert.IsType<ObjectResult>(result);
            await priceService.Received(1).GetPriceResultsForBasketAsync(
                Arg.Any<DateTime>(),
                Arg.Any<IEnumerable<PriceProduct>>(),
                Arg.Any<PriceClient>());
            Assert.NotNull(savedItems);
            var savedItem = Assert.Single(savedItems);
            Assert.Equal(5, savedItem.Quantity);
            Assert.Null(savedItem.UnitPrice);
            Assert.Null(savedItem.Price);
            Assert.Null(savedItem.Currency);

            var pricedProducts = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[1] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("SKU", priceProduct.ProductVariantSku);
            Assert.Equal("No", priceProduct.IsOutlet);
        }

        // These pin the same resolution matrix as BasketsApiControllerFlowTests, so the multipart
        // upload path cannot drift from the JSON path now that both go through
        // BasketDiscountCodeCoordinator.
        [Fact]
        public async Task Index_WhenTheUploadCarriesADiscountCode_SavesTheTrimmedCode()
        {
            var (result, savedDiscountCode, priceService) = await RunDiscountCodeScenarioAsync(
                new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "WINTER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "  SUMMER25  ");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
            await priceService.Received(1).GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public async Task Index_WhenTheUploadOmitsTheDiscountCode_KeepsThePersistedOne()
        {
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: false,
                requestedDiscountCode: null);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
        }

        [Fact]
        public async Task Index_WhenTheUploadSendsABlankDiscountCode_KeepsThePersistedOne()
        {
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "   ");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
        }

        [Fact]
        public async Task Index_WhenGrulaIsNotConfigured_IgnoresTheUploadedCodeAndKeepsThePersistedOne()
        {
            var (result, savedDiscountCode, priceService) = await RunDiscountCodeScenarioAsync(
                new AppSettings(),
                persistedDiscountCode: "WINTER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "SUMMER25");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("WINTER25", savedDiscountCode);
            await priceService.DidNotReceive().GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public async Task Index_WhenTheUploadRemovesTheDiscountCode_ClearsThePersistedOne()
        {
            // ASP.NET Core's ConvertEmptyStringToNull binds an empty form field to a null
            // model property while the key itself is still present on the request.
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: true,
                requestedDiscountCode: null);

            Assert.IsType<ObjectResult>(result);
            Assert.Null(savedDiscountCode);
        }

        // ---- Discount code enforcement: the multipart upload must resolve a code exactly as the JSON save does ----

        private static AppSettings EnforcedSettings() => new AppSettings
        {
            GrulaAccessToken = "test-token",
            GrulaEnvironmentId = Guid.NewGuid().ToString(),
            DiscountCodeEnforcementEnabled = true
        };

        private static IDiscountCodeValidator ValidatorAnswering(DiscountCodeValidation validation)
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<System.Threading.CancellationToken>())
                .Returns(Task.FromResult(validation));

            return validator;
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheUploadCarriesAValidCode_StoresTheCanonicalSpellingAndTheBuyersClient()
        {
            var clientId = Guid.NewGuid();
            var validator = ValidatorAnswering(DiscountCodeValidation.Valid("SUMMER25"));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), null, formHasDiscountCode: true, requestedDiscountCode: "summer25", validator, clientId);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, ((ObjectResult)outcome.Result).StatusCode);
            Assert.Equal("SUMMER25", outcome.SavedDiscountCode);
            Assert.Equal(clientId, outcome.SavedClientId);
            await validator.Received().ValidateAsync(clientId, "summer25", "token", Arg.Any<System.Threading.CancellationToken>());
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheUploadCarriesACodeThatCannotBeApplied_Returns400AndSavesNothing()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "STORED", formHasDiscountCode: true, requestedDiscountCode: "OTHER", validator, Guid.NewGuid());

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, ((ObjectResult)outcome.Result).StatusCode);
            Assert.False(outcome.WasSaved);
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheStoredCodeIsNoLongerApplicable_RemovesItAndReportsIt()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "SUMMER25", formHasDiscountCode: false, requestedDiscountCode: null, validator, Guid.NewGuid());

            var response = (Buyer.Web.Areas.Orders.ApiResponseModels.BasketResponseModel)((ObjectResult)outcome.Result).Value;
            Assert.True(outcome.WasSaved);
            Assert.Null(outcome.SavedDiscountCode);
            Assert.Null(outcome.SavedClientId);
            Assert.Null(response.DiscountCode);
            Assert.Equal("DiscountCodeRemovedFromBasket", response.DiscountCodeRemovedMessage);
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheStoredCodeCannotBeVerified_Returns400AndKeepsTheBasket()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "SUMMER25", formHasDiscountCode: false, requestedDiscountCode: null, validator, Guid.NewGuid());

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, ((ObjectResult)outcome.Result).StatusCode);
            Assert.False(outcome.WasSaved);
        }

        [Fact]
        public async Task Index_WithEnforcementOff_ValidatesNothingAndStoresNoClient()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();

            var outcome = await RunUploadScenarioAsync(
                new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                null, formHasDiscountCode: true, requestedDiscountCode: "typed", validator, Guid.NewGuid());

            Assert.Equal("typed", outcome.SavedDiscountCode);
            Assert.Null(outcome.SavedClientId);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        private static async Task<(IActionResult Result, string SavedDiscountCode, IPriceService PriceService)> RunDiscountCodeScenarioAsync(
            AppSettings appSettings,
            string persistedDiscountCode,
            bool formHasDiscountCode,
            string requestedDiscountCode)
        {
            var outcome = await RunUploadScenarioAsync(appSettings, persistedDiscountCode, formHasDiscountCode, requestedDiscountCode);

            return (outcome.Result, outcome.SavedDiscountCode, outcome.PriceService);
        }

        public sealed record UploadOutcome(IActionResult Result, string SavedDiscountCode, Guid? SavedClientId, IPriceService PriceService, bool WasSaved);

        public static async Task<UploadOutcome> RunUploadScenarioAsync(
            AppSettings appSettings,
            string persistedDiscountCode,
            bool formHasDiscountCode,
            string requestedDiscountCode,
            IDiscountCodeValidator validator = null,
            Guid? principalClientId = null)
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            string savedDiscountCode = null;
            Guid? savedClientId = null;
            var wasSaved = false;
            var existingBasket = new DomainBasket
            {
                Id = basketId,
                DiscountCode = persistedDiscountCode,
                Items = new[]
                {
                    new BasketItem { ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2 }
                }
            };
            var orderFileService = Substitute.For<IOrderFileService>();
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var inventoryRepository = Substitute.For<IInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(appSettings);

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[]
            {
                new OrderFileLine { Sku = "SKU", Quantity = 1 }
            });
            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId).Returns(Task.FromResult(existingBasket));
            productsRepository.GetProductsBySkusAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[]
                {
                    new Product { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" }
                }));
            inventoryRepository.GetStockAvailbleProductsByProductIdsAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                .Returns(Task.FromResult<IEnumerable<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>>(Array.Empty<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>()));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 10m, CurrencyCode = "EUR" } }
                }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(call =>
                {
                    wasSaved = true;
                    savedDiscountCode = call.ArgAt<string>(4);
                    savedClientId = call.ArgAt<Guid?>(5);
                    return Task.FromResult(new DomainBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<BasketItem>>(3).ToList() });
                });

            var formFields = formHasDiscountCode
                ? new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { ["DiscountCode"] = requestedDiscountCode ?? string.Empty }
                : new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            var httpContext = CreateHttpContext(basketId, formFields, principalClientId);

            var controller = new OrderFileApiController(
                orderFileService,
                productsRepository,
                basketRepository,
                Substitute.For<LinkGenerator>(),
                options,
                Substitute.For<IMediaService>(),
                Substitute.For<IMediaItemsRepository>(),
                Substitute.For<IOrdersRepository>(),
                inventoryRepository,
                Substitute.For<ILogger<OrderFileApiController>>(),
                TestLocalizer.Create<OrderResources>(),
                priceService,
                productsService,
                productColorsService,
                new PriceProductFactory(productsService, productColorsService, options),
                CreatePriceClientResolver(httpContext),
                CreateBasketRepricingService(priceService),
                validator ?? Substitute.For<IDiscountCodeValidator>())
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.Index(new UploadMediaRequestModel { File = Substitute.For<IFormFile>(), DiscountCode = requestedDiscountCode });

            return new UploadOutcome(result, savedDiscountCode, savedClientId, priceService, wasSaved);
        }

        // The controllers delegate the align/apply spine to BasketRepricingService, so these flow
        // tests drive the real one over a substituted IPriceService rather than stubbing it out.
        private static IBasketRepricingService CreateBasketRepricingService(IPriceService priceService)
        {
            return new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
        }

        private static IPriceClientResolver CreatePriceClientResolver(HttpContext httpContext)
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);
            return new ClaimsPriceClientResolver(httpContextAccessor);
        }

        private static DefaultHttpContext CreateHttpContext(Guid basketId, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> formFields = null, Guid? principalClientId = null)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={basketId}";
            context.Features.Set<IFormFeature>(new FormFeature(new FormCollection(formFields ?? new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>())));
            context.User = principalClientId.HasValue
                ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants.ClientIdClaimType, principalClientId.Value.ToString()) }, "test"))
                : new ClaimsPrincipal(new ClaimsIdentity());
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();
            return context;
        }
    }

    // Both the Buyer and the Seller controller are covered so the shared reprice orchestration
    // they now delegate to stays pinned from both ends. See BuyerOrderFileApiControllerFlowTests
    // for the shared rationale (an unpriced result must not retain the line's existing price).
    public class SellerOrderFileApiControllerFlowTests
    {
        [Fact]
        public async Task Index_WhenMergedLineHasAnUnpricedPricingResult_DoesNotRetainTheExistingPrice()
        {
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            IEnumerable<SellerBasketItem> savedItems = null;
            var existingBasket = new SellerBasket
            {
                Id = basketId,
                Items = new[]
                {
                    new SellerBasketItem
                    {
                        ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2,
                        ExternalReference = "reference", MoreInfo = "notes", UnitPrice = 10m, Price = 20m, Currency = "EUR"
                    }
                }
            };
            var orderFileService = Substitute.For<SellerIOrderFileService>();
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var inventoryRepository = Substitute.For<SellerIInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IPriceClientResolver>();
            var options = Options.Create(new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[]
            {
                new SellerOrderFileLine { Sku = "SKU", Quantity = 3, ExternalReference = "reference", MoreInfo = "notes" }
            });
            basketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), basketId).Returns(Task.FromResult(existingBasket));
            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[]
                {
                    new SellerProduct { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" }
                }));
            inventoryRepository.GetAvailbleProductsByProductIdsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                .Returns(Task.FromResult<IEnumerable<SellerInventoryItem>>(Array.Empty<SellerInventoryItem>()));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.InvalidPriceDrivers }
                }));
            priceClientResolver.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0) }));
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    savedItems = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList();
                    return Task.FromResult(new SellerBasket { Id = basketId, Items = savedItems });
                });

            var controller = new SellerOrderFileApiController(
                orderFileService,
                productsRepository,
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                Substitute.For<SellerIMediaItemsRepository>(),
                Substitute.For<SellerIOrdersRepository>(),
                inventoryRepository,
                Substitute.For<ILogger<SellerOrderFileApiController>>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                priceService,
                productsService,
                productColorsService,
                options,
                priceClientResolver,
                new SellerPriceProductFactory(productsService, productColorsService, options),
                CreateBasketRepricingService(priceService),
                Substitute.For<IDiscountCodeValidator>())
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
            };

            var result = await controller.Index(new SellerUploadMediaRequestModel { Id = basketId, ClientId = clientId, File = Substitute.For<IFormFile>() });

            Assert.IsType<ObjectResult>(result);
            await priceService.Received(1).GetPriceResultsForBasketAsync(
                Arg.Any<DateTime>(),
                Arg.Any<IEnumerable<PriceProduct>>(),
                Arg.Any<PriceClient>());
            await priceClientResolver.Received(1).ResolveAsync(clientId, Arg.Any<string>(), Arg.Any<string>());
            Assert.NotNull(savedItems);
            var savedItem = Assert.Single(savedItems);
            Assert.Equal(5, savedItem.Quantity);
            Assert.Null(savedItem.UnitPrice);
            Assert.Null(savedItem.Price);
            Assert.Null(savedItem.Currency);

            var pricedProducts = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[1] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("SKU", priceProduct.ProductVariantSku);
            Assert.Equal("No", priceProduct.IsOutlet);
        }

        // These pin the same resolution matrix as BasketsApiControllerFlowTests, so the multipart
        // upload path cannot drift from the JSON path now that both go through
        // BasketDiscountCodeCoordinator.
        [Fact]
        public async Task Index_WhenTheUploadCarriesADiscountCode_SavesTheTrimmedCode()
        {
            var (result, savedDiscountCode, priceService) = await RunDiscountCodeScenarioAsync(
                new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "WINTER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "  SUMMER25  ");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
            await priceService.Received(1).GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public async Task Index_WhenTheUploadOmitsTheDiscountCode_KeepsThePersistedOne()
        {
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: false,
                requestedDiscountCode: null);

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
        }

        [Fact]
        public async Task Index_WhenTheUploadSendsABlankDiscountCode_KeepsThePersistedOne()
        {
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "   ");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("SUMMER25", savedDiscountCode);
        }

        [Fact]
        public async Task Index_WhenGrulaIsNotConfigured_IgnoresTheUploadedCodeAndKeepsThePersistedOne()
        {
            var (result, savedDiscountCode, priceService) = await RunDiscountCodeScenarioAsync(
                new SellerAppSettings(),
                persistedDiscountCode: "WINTER25",
                formHasDiscountCode: true,
                requestedDiscountCode: "SUMMER25");

            Assert.IsType<ObjectResult>(result);
            Assert.Equal("WINTER25", savedDiscountCode);
            await priceService.DidNotReceive().GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
        }

        [Fact]
        public async Task Index_WhenTheUploadRemovesTheDiscountCode_ClearsThePersistedOne()
        {
            // ASP.NET Core's ConvertEmptyStringToNull binds an empty form field to a null
            // model property while the key itself is still present on the request.
            var (result, savedDiscountCode, _) = await RunDiscountCodeScenarioAsync(
                new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                persistedDiscountCode: "SUMMER25",
                formHasDiscountCode: true,
                requestedDiscountCode: null);

            Assert.IsType<ObjectResult>(result);
            Assert.Null(savedDiscountCode);
        }

        // ---- Discount code enforcement: the multipart upload must resolve a code exactly as the JSON save does ----

        private static SellerAppSettings EnforcedSettings() => new SellerAppSettings
        {
            GrulaAccessToken = "test-token",
            GrulaEnvironmentId = Guid.NewGuid().ToString(),
            DiscountCodeEnforcementEnabled = true
        };

        private static IDiscountCodeValidator ValidatorAnswering(DiscountCodeValidation validation)
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<System.Threading.CancellationToken>())
                .Returns(Task.FromResult(validation));

            return validator;
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheUploadCarriesAValidCode_StoresTheCanonicalSpellingAndTheClientTheSellerWorksFor()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Valid("SUMMER25"));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), null, formHasDiscountCode: true, requestedDiscountCode: "summer25", validator);

            Assert.Equal((int)System.Net.HttpStatusCode.OK, ((ObjectResult)outcome.Result).StatusCode);
            Assert.Equal("SUMMER25", outcome.SavedDiscountCode);
            Assert.Equal(outcome.ClientId, outcome.SavedClientId);
            await validator.Received().ValidateAsync(outcome.ClientId, "summer25", "token", Arg.Any<System.Threading.CancellationToken>());
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.Disabled, "DiscountCodeDisabled")]
        [InlineData(DiscountCodeValidationStatus.NotAssigned, "DiscountCodeNotAssignedToClient")]
        [InlineData(DiscountCodeValidationStatus.NotFound, "DiscountCodeInvalid")]
        public async Task Index_WithEnforcement_WhenTheUploadCarriesACodeThatCannotBeApplied_Returns400WithTheSpecificReasonAndSavesNothing(DiscountCodeValidationStatus status, string expectedMessage)
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(status));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "STORED", formHasDiscountCode: true, requestedDiscountCode: "OTHER", validator);

            var result = (ObjectResult)outcome.Result;
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(expectedMessage, (string)result.Value.GetType().GetProperty("Message").GetValue(result.Value));
            Assert.False(outcome.WasSaved);
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheStoredCodeIsNoLongerApplicable_RemovesItAndReportsIt()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "SUMMER25", formHasDiscountCode: false, requestedDiscountCode: null, validator);

            var response = (Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel)((ObjectResult)outcome.Result).Value;
            Assert.True(outcome.WasSaved);
            Assert.Null(outcome.SavedDiscountCode);
            Assert.Null(outcome.SavedClientId);
            Assert.Null(response.DiscountCode);
            Assert.Equal("DiscountCodeRemovedFromBasket", response.DiscountCodeRemovedMessage);
        }

        [Fact]
        public async Task Index_WithEnforcement_WhenTheStoredCodeCannotBeVerified_Returns400AndKeepsTheBasket()
        {
            var validator = ValidatorAnswering(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var outcome = await RunUploadScenarioAsync(EnforcedSettings(), "SUMMER25", formHasDiscountCode: false, requestedDiscountCode: null, validator);

            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, ((ObjectResult)outcome.Result).StatusCode);
            Assert.False(outcome.WasSaved);
        }

        [Fact]
        public async Task Index_WithEnforcementOff_ValidatesNothingAndStoresNoClient()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();

            var outcome = await RunUploadScenarioAsync(
                new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() },
                null, formHasDiscountCode: true, requestedDiscountCode: "typed", validator);

            Assert.Equal("typed", outcome.SavedDiscountCode);
            Assert.Null(outcome.SavedClientId);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        private static async Task<(IActionResult Result, string SavedDiscountCode, IPriceService PriceService)> RunDiscountCodeScenarioAsync(
            SellerAppSettings appSettings,
            string persistedDiscountCode,
            bool formHasDiscountCode,
            string requestedDiscountCode)
        {
            var outcome = await RunUploadScenarioAsync(appSettings, persistedDiscountCode, formHasDiscountCode, requestedDiscountCode);

            return (outcome.Result, outcome.SavedDiscountCode, outcome.PriceService);
        }

        public sealed record UploadOutcome(IActionResult Result, string SavedDiscountCode, Guid? SavedClientId, IPriceService PriceService, Guid ClientId, bool WasSaved);

        public static async Task<UploadOutcome> RunUploadScenarioAsync(
            SellerAppSettings appSettings,
            string persistedDiscountCode,
            bool formHasDiscountCode,
            string requestedDiscountCode,
            IDiscountCodeValidator validator = null)
        {
            Guid? savedClientId = null;
            var wasSaved = false;
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            string savedDiscountCode = null;
            var existingBasket = new SellerBasket
            {
                Id = basketId,
                DiscountCode = persistedDiscountCode,
                Items = new[]
                {
                    new SellerBasketItem { ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2 }
                }
            };
            var orderFileService = Substitute.For<SellerIOrderFileService>();
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var inventoryRepository = Substitute.For<SellerIInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IPriceClientResolver>();
            var options = Options.Create(appSettings);

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[]
            {
                new SellerOrderFileLine { Sku = "SKU", Quantity = 1 }
            });
            basketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), basketId).Returns(Task.FromResult(existingBasket));
            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[]
                {
                    new SellerProduct { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" }
                }));
            inventoryRepository.GetAvailbleProductsByProductIdsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                .Returns(Task.FromResult<IEnumerable<SellerInventoryItem>>(Array.Empty<SellerInventoryItem>()));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 10m, CurrencyCode = "EUR" } }
                }));
            priceClientResolver.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0) }));
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(call =>
                {
                    wasSaved = true;
                    savedDiscountCode = call.ArgAt<string>(4);
                    savedClientId = call.ArgAt<Guid?>(5);
                    return Task.FromResult(new SellerBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList() });
                });

            var formFields = formHasDiscountCode
                ? new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { ["DiscountCode"] = requestedDiscountCode ?? string.Empty }
                : new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();

            var controller = new SellerOrderFileApiController(
                orderFileService,
                productsRepository,
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                Substitute.For<SellerIMediaItemsRepository>(),
                Substitute.For<SellerIOrdersRepository>(),
                inventoryRepository,
                Substitute.For<ILogger<SellerOrderFileApiController>>(),
                TestLocalizer.Create<OrderResources>(),
                priceService,
                productsService,
                productColorsService,
                options,
                priceClientResolver,
                new SellerPriceProductFactory(productsService, productColorsService, options),
                CreateBasketRepricingService(priceService),
                validator ?? Substitute.For<IDiscountCodeValidator>())
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext(formFields) }
            };

            var result = await controller.Index(new SellerUploadMediaRequestModel { Id = basketId, ClientId = clientId, File = Substitute.For<IFormFile>(), DiscountCode = requestedDiscountCode });

            return new UploadOutcome(result, savedDiscountCode, savedClientId, priceService, clientId, wasSaved);
        }

        // The controllers delegate the align/apply spine to BasketRepricingService, so these flow
        // tests drive the real one over a substituted IPriceService rather than stubbing it out.
        private static IBasketRepricingService CreateBasketRepricingService(IPriceService priceService)
        {
            return new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
        }

        private static DefaultHttpContext CreateHttpContext(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> formFields = null)
        {
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };
            context.Features.Set<IFormFeature>(new FormFeature(new FormCollection(formFields ?? new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>())));
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