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
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerBasket = Seller.Web.Areas.Orders.DomainModels.Basket;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketsApiController = Seller.Web.Areas.Orders.ApiControllers.BasketsApiController;
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
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;
using SellerUploadMediaRequestModel = Seller.Web.Areas.Media.ApiRequestModels.UploadMediaRequestModel;
using StringValues = Microsoft.Extensions.Primitives.StringValues;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class SellerBasketsApiControllerDiscountCodeEnforcementTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static async Task<DiscountFlowResult> RunAsync(
            IDiscountCodeValidator validator,
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            Guid? clientId,
            bool hasItems = true)
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var saveCalls = 0;
            string savedDiscountCode = null;
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IBasketPriceClientResolver>();
            var options = Options.Create(new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            basketRepository.GetBasketByIdAsync("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(new SellerBasket { Id = basketId, DiscountCode = persistedDiscountCode }));
            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[] { new SellerProduct { Id = productId, Sku = "SKU", Name = "Product", PrimaryProductSku = "PRIMARY" } }));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.Priced, Price = new Price { CurrentPrice = 10m, CurrencyCode = "EUR" } }
                }));
            priceClientResolver.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0), DiscountCode = call.ArgAt<string>(1) }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedDiscountCode = call.ArgAt<string>(4);

                    return Task.FromResult(new SellerBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList() });
                });

            var controller = new SellerBasketsApiController(
                basketRepository,
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
                validator)
            {
                ControllerContext = new ControllerContext { HttpContext = SellerFlowHelpers.CreateHttpContext() }
            };

            var request = new SellerSaveBasketRequestModel
            {
                Id = basketId,
                ClientId = clientId,
                Items = hasItems
                    ? new[] { new SellerBasketItemRequestModel { ProductId = productId, Sku = "SKU", Name = "Product", Quantity = 1 } }
                    : Array.Empty<SellerBasketItemRequestModel>()
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
                PricingCalls = priceService.ReceivedCalls().Count(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync)),
                PricedWithDiscountCode = priceClientResolver.ReceivedCalls()
                    .Where(x => x.GetMethodInfo().Name == nameof(IPriceClientResolver.ResolveAsync))
                    .Select(x => x.GetArguments()[1] as string)
                    .FirstOrDefault()
            };
        }

        [Fact]
        public async Task Index_WhenANewCodeIsApplicable_ValidatesItForTheSelectedClientAndStoresTheCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "summer25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            var flow = await RunAsync(validator, true, "summer25", null, ClientId);

            Assert.Equal("SUMMER25", flow.SavedDiscountCode);
            Assert.Equal("SUMMER25", flow.PricedWithDiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "summer25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.NotApplicable, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.Disabled, "DiscountCodeDisabled")]
        [InlineData(DiscountCodeValidationStatus.NotAssigned, "DiscountCodeNotAssignedToClient")]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown, "DiscountCodeClientRequired")]
        [InlineData(DiscountCodeValidationStatus.Unavailable, "DiscountCodeCouldNotBeVerified")]
        public async Task Index_WhenANewCodeCannotBeApplied_RejectsWithTheSpecificReasonAndSavesNothing(DiscountCodeValidationStatus status, string expectedMessage)
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(status), true, "NEW25", null, ClientId);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(expectedMessage, flow.Message);
            Assert.Equal(0, flow.SaveCalls);
            Assert.Equal(0, flow.PricingCalls);
        }

        [Fact]
        public async Task Index_WhenANewCodeIsAppliedWithoutAClient_RejectsAskingForAClientWithoutAnyLookup()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            var validator = new DiscountCodeValidator(lookup, Substitute.For<ILogger<DiscountCodeValidator>>());

            var flow = await RunAsync(validator, true, "NEW25", null, null);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("DiscountCodeClientRequired", flow.Message);
            Assert.Equal(0, flow.SaveCalls);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_WhenAReplacementCodeIsRejected_LeavesTheStoredBasketAlone()
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned), true, "NEW25", "STORED25", ClientId);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal(0, flow.SaveCalls);
        }

        [Theory]
        [InlineData(true, "STORED25")]
        [InlineData(true, "stored25")]
        [InlineData(true, "   ")]
        [InlineData(false, null)]
        public async Task Index_WhenTheStoredCodeIsKept_SurvivesRevocationWithoutAnyValidationEvenForAnotherClient(bool hasDiscountCode, string requested)
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            // A different client than the one the basket was started for: the stored code is retained as it is (C9/C10).
            var flow = await RunAsync(validator, hasDiscountCode, requested, "STORED25", Guid.NewGuid());

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

            var flow = await RunAsync(validator, false, null, "STORED25", ClientId);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
        }

        [Fact]
        public async Task Index_ForAnExplicitRemoval_ClearsTheCodeWithoutValidating()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var flow = await RunAsync(validator, true, null, "STORED25", ClientId);

            Assert.Null(flow.SavedDiscountCode);
            Assert.Null(flow.PricedWithDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }

    public class SellerOrderFileApiControllerDiscountCodeEnforcementTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static async Task<DiscountFlowResult> RunAsync(
            IDiscountCodeValidator validator,
            bool formHasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            Guid clientId)
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var saveCalls = 0;
            string savedDiscountCode = null;
            var orderFileService = Substitute.For<SellerIOrderFileService>();
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var inventoryRepository = Substitute.For<SellerIInventoryRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IBasketPriceClientResolver>();
            var options = Options.Create(new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            orderFileService.ImportOrderLines(Arg.Any<IFormFile>()).Returns(new[] { new SellerOrderFileLine { Sku = "SKU", Quantity = 1 } });
            basketRepository.GetBasketByIdAsync(Arg.Any<string>(), Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(new SellerBasket
                {
                    Id = basketId,
                    DiscountCode = persistedDiscountCode,
                    Items = new[] { new SellerBasketItem { ProductId = productId, ProductSku = "SKU", ProductName = "Product", Quantity = 2 } }
                }));
            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[] { new SellerProduct { Id = productId, Sku = "SKU", PrimaryProductSku = "PRIMARY" } }));
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
                .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0), DiscountCode = call.ArgAt<string>(1) }));
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedDiscountCode = call.ArgAt<string>(4);

                    return Task.FromResult(new SellerBasket { Id = basketId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList() });
                });

            var formFields = formHasDiscountCode
                ? new Dictionary<string, StringValues> { ["DiscountCode"] = requestedDiscountCode ?? string.Empty }
                : new Dictionary<string, StringValues>();

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
                new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>()),
                validator)
            {
                ControllerContext = new ControllerContext { HttpContext = SellerFlowHelpers.CreateHttpContext(formFields) }
            };

            var result = await controller.Index(new SellerUploadMediaRequestModel { Id = basketId, ClientId = clientId, File = Substitute.For<IFormFile>(), DiscountCode = requestedDiscountCode });

            return new DiscountFlowResult
            {
                Result = result,
                SaveCalls = saveCalls,
                SavedDiscountCode = savedDiscountCode,
                PricingCalls = priceService.ReceivedCalls().Count(x => x.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync)),
                PricedWithDiscountCode = priceClientResolver.ReceivedCalls()
                    .Where(x => x.GetMethodInfo().Name == nameof(IPriceClientResolver.ResolveAsync))
                    .Select(x => x.GetArguments()[1] as string)
                    .FirstOrDefault()
            };
        }

        [Fact]
        public async Task Index_WhenTheUploadCarriesANewApplicableCode_ValidatesItForTheSelectedClientAndStoresTheCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "new25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("NEW25")));

            var flow = await RunAsync(validator, true, " new25 ", "STORED25", ClientId);

            Assert.Equal("NEW25", flow.SavedDiscountCode);
            Assert.Equal("NEW25", flow.PricedWithDiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "new25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound, "DiscountCodeInvalid")]
        [InlineData(DiscountCodeValidationStatus.Disabled, "DiscountCodeDisabled")]
        [InlineData(DiscountCodeValidationStatus.NotAssigned, "DiscountCodeNotAssignedToClient")]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown, "DiscountCodeClientRequired")]
        [InlineData(DiscountCodeValidationStatus.Unavailable, "DiscountCodeCouldNotBeVerified")]
        public async Task Index_WhenTheUploadCarriesACodeThatCannotBeApplied_RejectsWithTheSpecificReasonAndLeavesTheBasketUnsaved(DiscountCodeValidationStatus status, string expectedMessage)
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(status), true, "NEW25", "STORED25", ClientId);

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
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);

            var flow = await RunAsync(validator, formHasDiscountCode, requested, "STORED25", ClientId);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.Equal("STORED25", flow.PricedWithDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task Index_ForAnExplicitRemoval_ClearsTheCodeWithoutValidating()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);

            var flow = await RunAsync(validator, true, null, "STORED25", ClientId);

            Assert.Null(flow.SavedDiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }

    internal static class SellerFlowHelpers
    {
        public static DefaultHttpContext CreateHttpContext(Dictionary<string, StringValues> formFields = null)
        {
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };

            context.Features.Set<IFormFeature>(new FormFeature(new FormCollection(formFields ?? new Dictionary<string, StringValues>())));

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
