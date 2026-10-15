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
using SellerBasketCheckoutApiController = Seller.Web.Areas.Orders.ApiControllers.BasketCheckoutApiController;
using SellerBasketsApiController = Seller.Web.Areas.Orders.ApiControllers.BasketsApiController;
using SellerCheckoutBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.CheckoutBasketRequestModel;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerIBasketService = Seller.Web.Areas.Orders.Services.Basket.IBasketService;
using SellerIClientsRepository = Seller.Web.Shared.Repositories.Clients.IClientsRepository;
using SellerIIdentityRepository = Seller.Web.Shared.Repositories.Identity.IIdentityRepository;
using SellerIInventoryAreaRepository = Seller.Web.Areas.Inventory.Repositories.Inventories.IInventoryRepository;
using SellerIOutletRepository = Seller.Web.Areas.Inventory.Repositories.IOutletRepository;
using SellerIUserApprovalsRepository = Seller.Web.Areas.Shared.Repositories.UserApprovals.IUserApprovalsRepository;
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
            Guid? savedClientId = null;
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
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<Guid?>(), Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedClientId = call.ArgAt<Guid?>(3);
                    savedDiscountCode = call.ArgAt<string>(5);

                    return Task.FromResult(new SellerBasket { Id = basketId, ClientId = savedClientId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<SellerBasketItem>>(4).ToList() });
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
                SavedClientId = savedClientId,
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
        public async Task Index_WhenTheBasketIsSavedForAnotherClient_DoesNotCarryTheStoredCodeOver(bool hasDiscountCode, string requested)
        {
            // The code was applied for client A; the same basket is now saved for client B, to whom it is not assigned. The
            // seller's edit is saved, priced without the code, and the 422 says why the prices changed.
            var otherClientId = Guid.NewGuid();
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(otherClientId, "STORED25", "token")
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned)));

            var flow = await RunAsync(validator, hasDiscountCode, requested, "STORED25", otherClientId);

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, flow.StatusCode);
            Assert.Equal("DiscountCodeRemovedPricesUpdated", flow.Message);
            Assert.Equal(1, flow.SaveCalls);
            Assert.Null(flow.SavedDiscountCode);
            Assert.Equal(otherClientId, flow.SavedClientId);
            Assert.Equal(1, flow.PricingCalls);
            Assert.Null(flow.PricedWithDiscountCode);
            Assert.NotNull(flow.ResponseBasketId);
            Assert.Null(flow.ResponseDiscountCode);
            await validator.Received(1).ValidateAsync(otherClientId, "STORED25", "token");
        }

        [Theory]
        [InlineData(true, "stored25")]
        [InlineData(false, null)]
        public async Task Index_WhenTheStoredCodeIsStillApplicableForTheClient_KeepsItWithTheCanonicalSpelling(bool hasDiscountCode, string requested)
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "STORED25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("Stored25")));

            var flow = await RunAsync(validator, hasDiscountCode, requested, "STORED25", ClientId);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("Stored25", flow.SavedDiscountCode);
            Assert.Equal("Stored25", flow.PricedWithDiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsKeptWhileTheLookupIsDown_StillSavesAndPricesWithIt()
        {
            var flow = await RunAsync(LookupDown.CreateValidator(), false, null, "STORED25", ClientId);

            Assert.Equal((int)HttpStatusCode.OK, flow.StatusCode);
            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.Equal("STORED25", flow.PricedWithDiscountCode);
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
            Guid? savedClientId = null;
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
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<Guid?>(), Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    saveCalls++;
                    savedClientId = call.ArgAt<Guid?>(3);
                    savedDiscountCode = call.ArgAt<string>(5);

                    return Task.FromResult(new SellerBasket { Id = basketId, ClientId = savedClientId, DiscountCode = savedDiscountCode, Items = call.ArgAt<IEnumerable<SellerBasketItem>>(4).ToList() });
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
                SavedClientId = savedClientId,
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
        public async Task Index_WhenTheUploadIsForAClientTheStoredCodeIsNotAssignedTo_DoesNotCarryTheCodeOver(bool formHasDiscountCode, string requested)
        {
            var otherClientId = Guid.NewGuid();
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(otherClientId, "STORED25", "token")
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned)));

            var flow = await RunAsync(validator, formHasDiscountCode, requested, "STORED25", otherClientId);

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, flow.StatusCode);
            Assert.Equal("DiscountCodeRemovedPricesUpdated", flow.Message);
            Assert.Equal(1, flow.SaveCalls);
            Assert.Null(flow.SavedDiscountCode);
            Assert.Equal(otherClientId, flow.SavedClientId);
            Assert.Equal(1, flow.PricingCalls);
            Assert.Null(flow.PricedWithDiscountCode);
            Assert.NotNull(flow.ResponseBasketId);
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeWasRevoked_SavesTheUploadWithoutIt()
        {
            var flow = await RunAsync(DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled), false, null, "STORED25", ClientId);

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, flow.StatusCode);
            Assert.Equal("DiscountCodeRemovedPricesUpdated", flow.Message);
            Assert.Equal(1, flow.SaveCalls);
            Assert.Null(flow.SavedDiscountCode);
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsKeptWhileTheLookupIsDown_StillSavesTheUploadAndPricesWithIt()
        {
            var flow = await RunAsync(LookupDown.CreateValidator(), false, null, "STORED25", ClientId);

            Assert.Equal((int)HttpStatusCode.OK, flow.StatusCode);
            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("STORED25", flow.SavedDiscountCode);
            Assert.Equal("STORED25", flow.PricedWithDiscountCode);
        }

        [Fact]
        public async Task Index_WhenTheStoredCodeIsStillApplicableForTheClient_RepricesWithItsCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "STORED25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("Stored25")));

            var flow = await RunAsync(validator, true, "stored25", "STORED25", ClientId);

            Assert.Equal(1, flow.SaveCalls);
            Assert.Equal("Stored25", flow.SavedDiscountCode);
            Assert.Equal("Stored25", flow.PricedWithDiscountCode);
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

    public class SellerBasketCheckoutApiControllerDiscountCodeEnforcementTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static async Task<(DiscountFlowResult Flow, int CheckoutCalls)> CheckoutAsync(
            IDiscountCodeValidator validator,
            string storedDiscountCode,
            Guid? pricedForClientId,
            Guid clientId,
            bool isGrulaConfigured = true)
        {
            var basketId = Guid.NewGuid();
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var clientsRepository = Substitute.For<SellerIClientsRepository>();
            var options = Options.Create(isGrulaConfigured
                ? new SellerAppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() }
                : new SellerAppSettings());

            // The basket as its last save stored it: priced for pricedForClientId, and checked out below for clientId.
            basketRepository.GetBasketByIdAsync("token", Arg.Any<string>(), basketId)
                .Returns(Task.FromResult(new SellerBasket
                {
                    Id = basketId,
                    ClientId = pricedForClientId,
                    DiscountCode = storedDiscountCode,
                    Items = new[] { new SellerBasketItem { ProductId = Guid.NewGuid(), ProductSku = "SKU", ProductName = "Product", Quantity = 1, UnitPrice = 9m, Price = 9m } }
                }));
            clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>(), clientId)
                .Returns(Task.FromResult(new SellerClient { Id = clientId, Email = "client@example.com", OrganisationId = Guid.NewGuid() }));

            var controller = new SellerBasketCheckoutApiController(
                basketRepository,
                TestLocalizer.Create<OrderResources>(),
                TestLocalizer.Create<ClientResources>(),
                Substitute.For<SellerIUserApprovalsRepository>(),
                clientsRepository,
                Substitute.For<SellerIIdentityRepository>(),
                Substitute.For<SellerIBasketService>(),
                Substitute.For<SellerIInventoryAreaRepository>(),
                Substitute.For<SellerIOutletRepository>(),
                validator,
                options,
                Substitute.For<ILogger<SellerBasketCheckoutApiController>>())
            {
                ControllerContext = new ControllerContext { HttpContext = SellerFlowHelpers.CreateHttpContext() }
            };

            var result = await controller.Checkout(new SellerCheckoutBasketRequestModel { BasketId = basketId, ClientId = clientId });

            return (
                new DiscountFlowResult { Result = result },
                basketRepository.ReceivedCalls().Count(x => x.GetMethodInfo().Name == nameof(SellerIBasketRepository.CheckoutBasketAsync)));
        }

        [Theory]
        [InlineData("STORED25")]
        [InlineData(null)]
        public async Task Checkout_OfABasketPricedForAnotherClient_IsRefusedEvenWhenItsCodeIsApplicableForBoth(string storedDiscountCode)
        {
            // The code is assigned to client A and client B. The basket was priced for A - with A's currency, country and
            // postal code - and is checked out for B, so B would be ordered A's prices.
            var otherClientId = Guid.NewGuid();

            var (flow, checkoutCalls) = await CheckoutAsync(DiscountCodeValidators.AlwaysValid(), storedDiscountCode, otherClientId, ClientId);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("BasketPricedForAnotherClient", flow.Message);
            Assert.Equal(0, checkoutCalls);
        }

        [Fact]
        public async Task Checkout_OfABasketSavedBeforeItsClientWasRecorded_IsRefusedUntilItIsSavedForTheClient()
        {
            var (flow, checkoutCalls) = await CheckoutAsync(DiscountCodeValidators.AlwaysValid(), "STORED25", null, ClientId);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("BasketPricedForAnotherClient", flow.Message);
            Assert.Equal(0, checkoutCalls);
        }

        [Fact]
        public async Task Checkout_WithoutPricing_DoesNotRequireTheBasketToHaveBeenPricedForTheClient()
        {
            // Without Grula every save clears the prices, so the basket carries nothing calculated for any one client.
            var (flow, checkoutCalls) = await CheckoutAsync(DiscountCodeValidators.AlwaysValid(), null, null, ClientId, isGrulaConfigured: false);

            Assert.Equal((int)HttpStatusCode.Accepted, flow.StatusCode);
            Assert.Equal(1, checkoutCalls);
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeWasUnassignedFromTheClientSinceTheBasketWasPriced_IsRefused()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "STORED25", "token")
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned)));

            var (flow, checkoutCalls) = await CheckoutAsync(validator, "STORED25", ClientId, ClientId);

            Assert.Equal((int)HttpStatusCode.BadRequest, flow.StatusCode);
            Assert.Equal("DiscountCodeNotAssignedToClient", flow.Message);
            Assert.Equal(0, checkoutCalls);
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeIsApplicableForTheClient_PlacesTheOrder()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var (flow, checkoutCalls) = await CheckoutAsync(validator, "STORED25", ClientId, ClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, flow.StatusCode);
            Assert.Equal(1, checkoutCalls);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task Checkout_WhenTheStoredCodeCannotBeVerifiedBecauseTheLookupIsDown_PlacesTheOrder()
        {
            // The basket was priced with the code when it was applicable; an outage of the lookup does not block the order.
            var (flow, checkoutCalls) = await CheckoutAsync(LookupDown.CreateValidator(), "STORED25", ClientId, ClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, flow.StatusCode);
            Assert.Equal(1, checkoutCalls);
        }

        [Fact]
        public async Task Checkout_OfABasketWithoutACode_DoesNotValidateAnything()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Unavailable);

            var (flow, checkoutCalls) = await CheckoutAsync(validator, null, ClientId, ClientId);

            Assert.Equal((int)HttpStatusCode.Accepted, flow.StatusCode);
            Assert.Equal(1, checkoutCalls);
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
