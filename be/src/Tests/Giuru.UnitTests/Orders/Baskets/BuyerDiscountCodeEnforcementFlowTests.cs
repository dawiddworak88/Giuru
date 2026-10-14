using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.Services.OrderFiles;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Repositories.Inventory;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
using System.Threading.Tasks;
using DomainBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using StringValues = Microsoft.Extensions.Primitives.StringValues;

namespace Giuru.UnitTests.Orders.Baskets
{
    /// <summary>What a save or upload did, as far as discount code enforcement is concerned.</summary>
    public sealed class DiscountFlowResult
    {
        public IActionResult Result { get; init; }
        public int SaveCalls { get; init; }
        public string SavedDiscountCode { get; init; }
        public string PricedWithDiscountCode { get; init; }
        public int PricingCalls { get; init; }
        public bool IssuedDiscountCodeBinding { get; init; }

        public int StatusCode => (Result as ObjectResult)?.StatusCode ?? 0;

        public string Message => Result is ObjectResult { Value: not null } objectResult
            ? objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value) as string
            : null;
    }

    public class BuyerBasketsApiControllerDiscountCodeEnforcementTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static async Task<DiscountFlowResult> RunAsync(
            IDiscountCodeValidator validator,
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems = true,
            Guid? persistedCodeClientId = null,
            bool hasBinding = true,
            bool isGrulaConfigured = true)
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var saveCalls = 0;
            string savedDiscountCode = null;
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(new AppSettings { GrulaAccessToken = isGrulaConfigured ? "test-token" : null, GrulaEnvironmentId = Guid.NewGuid().ToString() });

            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(new DomainBasket { Id = basketId, DiscountCode = persistedDiscountCode }));
            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[] { new Product { Id = productId, Sku = "SKU", Name = "Product", PrimaryProductSku = "PRIMARY" } }));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 10m, CurrencyCode = "EUR" } }
                }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedDiscountCode = call.ArgAt<string>(4);

                    return Task.FromResult(new DomainBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<BasketItem>>(3).ToList() });
                });

            var httpContext = BuyerFlowHelpers.CreateHttpContext(basketId, ClientId);

            if (hasBinding && persistedDiscountCode is not null)
            {
                DiscountCodeBindingTestHelper.AddProof(httpContext, basketId, persistedCodeClientId ?? ClientId, persistedDiscountCode);
            }

            var controller = new BasketsApiController(
                basketRepository,
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
                BuyerFlowHelpers.CreatePriceClientResolver(httpContext),
                new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>()),
                validator,
                DiscountCodeBindingTestHelper.Create(httpContext))
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var request = new SaveBasketRequestModel
            {
                Id = basketId,
                Items = hasItems
                    ? new[] { new BasketItemRequestModel { ProductId = productId, Sku = "SKU", Name = "Product", Quantity = 1 } }
                    : Array.Empty<BasketItemRequestModel>()
            };

            if (hasDiscountCode)
            {
                request.DiscountCode = requestedDiscountCode;
            }

            var result = await controller.Index(request);

            return new DiscountFlowResult
            {
                Result = result,
                SaveCalls = saveCalls,
                SavedDiscountCode = savedDiscountCode,
                IssuedDiscountCodeBinding = DiscountCodeBindingTestHelper.HasNewBinding(httpContext, basketId, ClientId, savedDiscountCode),
                PricingCalls = priceService.ReceivedCalls().Count(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync)),
                PricedWithDiscountCode = priceService.ReceivedCalls()
                    .Where(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                    .Select(x => (x.GetArguments()[2] as PriceClient)?.DiscountCode)
                    .FirstOrDefault()
            };
        }

        [Fact]
        public async Task Index_WhenANewCodeIsApplicable_ValidatesItForTheBuyersOwnClientAndStoresTheCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "summer25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            var flow = await RunAsync(validator, true, "summer25", null);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("SUMMER25", flow.SavedDiscountCode);
            Assert.Equal("SUMMER25", flow.PricedWithDiscountCode);
            Assert.True(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "summer25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        public async Task Index_WhenANewCodeCannotBeApplied_RejectsWithTheSameGenericMessageForEveryReasonAndSavesNothing(DiscountCodeValidationStatus status)
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(status), true, "NEW25", null);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("DiscountCodeInvalid", flow.Message);
            Assert.Equal(0, flow.SaveCalls);
            Assert.Equal(0, flow.PricingCalls);
        }

        [Fact]
        public async Task Index_WhenANewCodeCannotBeVerified_RejectsWithTheRetryMessageAndSavesNothing()
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Unavailable), true, "NEW25", null);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("DiscountCodeCouldNotBeVerified", flow.Message);
            Assert.Equal(0, flow.SaveCalls);
        }

        [Fact]
        public async Task Index_WhenAReplacementCodeIsRejected_LeavesTheStoredBasketAlone()
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned), true, "NEW25", "STORED25");

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(0, flow.SaveCalls);
        }

        [Fact]
        public async Task Index_WhenAReplacementCodeIsApplicable_StoresItInPlaceOfTheStoredOne()
        {
            var flow = await RunAsync(DiscountCodeValidators.AlwaysValid(), true, "NEW25", "STORED25");

            Assert.Equal("NEW25", flow.SavedDiscountCode);
            Assert.Equal("NEW25", flow.PricedWithDiscountCode);
        }

        [Theory]
        [InlineData(true, "STORED25")]
        [InlineData(true, "stored25")]
        [InlineData(true, "   ")]
        [InlineData(false, null)]
        public async Task Index_WhenTheStoredCodeIsKept_SurvivesRevocationWithoutAnyValidationAndRepricesWithTheStoredSpelling(bool hasDiscountCode, string requested)
        {
            // Every status the validator could answer with is a revocation; the stored code must not even ask.
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);

            var flow = await RunAsync(validator, hasDiscountCode, requested, "STORED25");

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.Equal("STORED25", flow.PricedWithDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsKeptWhileTheLookupIsDown_StillSavesWithoutAskingTheLookup()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns<Task<DiscountCodeValidation>>(_ => throw new InvalidOperationException("Client.Api is down"));

            var flow = await RunAsync(validator, false, null, "STORED25");

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
        }

        [Theory]
        [InlineData(true, "STORED25")]
        [InlineData(true, " stored25 ")]
        [InlineData(true, "   ")]
        [InlineData(false, null)]
        public async Task Index_AfterSwitchingAccounts_RevalidatesTheStoredCodeAndRejectsAnUnassignedCode(bool hasDiscountCode, string requested)
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, hasDiscountCode, requested, "STORED25", persistedCodeClientId: Guid.NewGuid());

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(0, flow.SaveCalls);
            Assert.Equal(0, flow.PricingCalls);
            Assert.False(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task Index_ForALegacyBasket_ValidatesItsCodeBeforeIssuingABinding()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var flow = await RunAsync(validator, false, null, "STORED25", hasBinding: false);

            Assert.Equal(1, flow.SaveCalls);
            Assert.True(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public async Task Index_WhenAnUnboundCodeIsRetainedWithoutPricing_DoesNotGrantAnExemption(bool hasItems, bool isGrulaConfigured)
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, false, null, "STORED25", hasItems: hasItems,
                hasBinding: false, isGrulaConfigured: isGrulaConfigured);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.False(flow.IssuedDiscountCodeBinding);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_ForAnExplicitRemoval_ClearsTheCodeAndRepricesWithoutOne()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, true, null, "STORED25");

            Assert.Equal(1, flow.SaveCalls);
            Assert.Null(flow.SavedDiscountCode);
            Assert.Null(flow.PricedWithDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_WhenANewCodeIsAppliedToAnEmptyBasket_RejectsWithTheEmptyBasketMessageWithoutValidating()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var flow = await RunAsync(validator, true, "NEW25", null, hasItems: false);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("DiscountCodeRequiresBasketItems", flow.Message);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }

    public class BuyerOrderFileApiControllerDiscountCodeEnforcementTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static async Task<DiscountFlowResult> RunAsync(
            IDiscountCodeValidator validator,
            bool formHasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            Guid? persistedCodeClientId = null,
            bool hasBinding = true)
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var saveCalls = 0;
            string savedDiscountCode = null;
            var orderFileService = Substitute.For<IOrderFileService>();
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var inventoryRepository = Substitute.For<IInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[] { new OrderFileLine { Sku = "SKU", Quantity = 1 } });
            basketRepository.GetBasketById("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(new DomainBasket
                {
                    Id = basketId,
                    DiscountCode = persistedDiscountCode,
                    Items = new[] { new BasketItem { ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2 } }
                }));
            productsRepository.GetProductsBySkusAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[] { new Product { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" } }));
            inventoryRepository.GetStockAvailbleProductsByProductIdsAsync("token", Arg.Any<string>(), Arg.Any<IEnumerable<Guid>>())
                .Returns(Task.FromResult<IEnumerable<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>>(Array.Empty<Buyer.Web.Shared.DomainModels.Inventory.InventoryItem>()));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 10m, CurrencyCode = "EUR" } }
                }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedDiscountCode = call.ArgAt<string>(4);

                    return Task.FromResult(new DomainBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<BasketItem>>(3).ToList() });
                });

            var formFields = formHasDiscountCode
                ? new Dictionary<string, StringValues> { ["DiscountCode"] = requestedDiscountCode ?? string.Empty }
                : new Dictionary<string, StringValues>();
            var httpContext = BuyerFlowHelpers.CreateHttpContext(basketId, ClientId, formFields);

            if (hasBinding && persistedDiscountCode is not null)
            {
                DiscountCodeBindingTestHelper.AddProof(httpContext, basketId, persistedCodeClientId ?? ClientId, persistedDiscountCode);
            }

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
                BuyerFlowHelpers.CreatePriceClientResolver(httpContext),
                new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>()),
                validator,
                DiscountCodeBindingTestHelper.Create(httpContext))
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.Index(new UploadMediaRequestModel { File = Substitute.For<IFormFile>(), DiscountCode = requestedDiscountCode });

            return new DiscountFlowResult
            {
                Result = result,
                SaveCalls = saveCalls,
                SavedDiscountCode = savedDiscountCode,
                IssuedDiscountCodeBinding = DiscountCodeBindingTestHelper.HasNewBinding(httpContext, basketId, ClientId, savedDiscountCode),
                PricingCalls = priceService.ReceivedCalls().Count(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync)),
                PricedWithDiscountCode = priceService.ReceivedCalls()
                    .Where(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                    .Select(x => (x.GetArguments()[2] as PriceClient)?.DiscountCode)
                    .FirstOrDefault()
            };
        }

        [Fact]
        public async Task Index_WhenTheUploadCarriesANewApplicableCode_ValidatesItForTheBuyersOwnClientAndStoresTheCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "new25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("NEW25")));

            var flow = await RunAsync(validator, true, " new25 ", "STORED25");

            Assert.Equal("NEW25", flow.SavedDiscountCode);
            Assert.Equal("NEW25", flow.PricedWithDiscountCode);
            Assert.True(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "new25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.Disabled, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.NotAssigned, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.Unavailable, "DiscountCodeCouldNotBeVerified")]
        public async Task Index_WhenTheUploadCarriesACodeThatCannotBeApplied_RejectsAndLeavesTheBasketUnsaved(DiscountCodeValidationStatus status, string expectedMessage)
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(status), true, "NEW25", "STORED25");

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(expectedMessage, flow.Message);
            Assert.Equal(0, flow.SaveCalls);
            Assert.Equal(0, flow.PricingCalls);
        }

        [Theory]
        [InlineData(true, "STORED25")]
        [InlineData(true, "stored25")]
        [InlineData(true, "   ")]
        [InlineData(false, null)]
        public async Task Index_WhenTheStoredCodeIsKept_SurvivesRevocationWithoutAnyValidationAndRepricesWithTheStoredSpelling(bool formHasDiscountCode, string requested)
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, formHasDiscountCode, requested, "STORED25");

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.Equal("STORED25", flow.PricedWithDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_ForAnExplicitRemoval_ClearsTheCodeWithoutValidating()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, true, null, "STORED25");

            Assert.Equal(1, flow.SaveCalls);
            Assert.Null(flow.SavedDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Theory]
        [InlineData(true, "stored25")]
        [InlineData(false, null)]
        public async Task Index_AfterSwitchingAccounts_RejectsAnUploadWithAnotherClientsCode(bool formHasDiscountCode, string requested)
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, formHasDiscountCode, requested, "STORED25", persistedCodeClientId: Guid.NewGuid());

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(0, flow.SaveCalls);
            Assert.Equal(0, flow.PricingCalls);
            Assert.False(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task Index_ForALegacyBasket_ValidatesTheStoredCodeBeforeBindingTheUpload()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var flow = await RunAsync(validator, false, null, "STORED25", hasBinding: false);

            Assert.Equal(1, flow.SaveCalls);
            Assert.True(flow.IssuedDiscountCodeBinding);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }
    }

    internal static class BuyerFlowHelpers
    {
        public static IBasketPriceClientResolver CreatePriceClientResolver(HttpContext httpContext)
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            return new ClaimsPriceClientResolver(httpContextAccessor);
        }

        public static DefaultHttpContext CreateHttpContext(Guid basketId, Guid clientId, Dictionary<string, StringValues> formFields = null)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={basketId}";
            context.Features.Set<IFormFeature>(new FormFeature(new FormCollection(formFields ?? new Dictionary<string, StringValues>())));
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, clientId.ToString()) }, "test"));

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
