using Buyer.Web.Areas.Orders.ApiControllers;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using DomainBasket = Buyer.Web.Areas.Orders.DomainModels.Basket;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Areas.Products.Services.ProductColors;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Pricing.DomainModels;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Pricing.Services;
using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
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
    public class BuyerBasketsApiControllerFlowTests
    {
        [Fact]
        public async Task Index_WhenBasketContainsAnUnresolvableSku_SavesThatLineUnpricedAndPricesResolvedLines()
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            IEnumerable<BasketItem> savedItems = null;
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(new AppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[]
                {
                    new Product { Id = productId, Sku = "RESOLVED", Name = "Canonical product name", PrimaryProductSku = "PRIMARY" }
                }));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult
                    {
                        Status = PriceLookupStatus.Priced,
                        Price = new Price { CurrentPrice = 12m, CurrencyCode = "EUR" }
                    }
                }));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    savedItems = call.ArgAt<IEnumerable<BasketItem>>(3).ToList();
                    return Task.FromResult(new DomainBasket { Id = basketId, Items = savedItems });
                });

            var httpContext = CreateHttpContext(basketId);

            var controller = new BasketsApiController(
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                productsRepository,
                priceService,
                productsService,
                productColorsService,
                options,
                Substitute.For<ILogger<BasketsApiController>>(),
                new PriceProductFactory(productsService, productColorsService, options),
                CreatePriceClientResolver(httpContext),
                CreateBasketRepricingService(priceService))
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.Index(new SaveBasketRequestModel
            {
                DiscountCode = "SUMMER25",
                Items = new[]
                {
                    new BasketItemRequestModel { ProductId = productId, Sku = "RESOLVED", Name = "Spoofed product name", Quantity = 2, UnitPrice = 0.01m, Price = 0.02m, Currency = "FAKE" },
                    new BasketItemRequestModel { ProductId = Guid.NewGuid(), Sku = "UNKNOWN", Name = "Unknown", Quantity = 1, UnitPrice = 999m, Price = 999m, Currency = "USD" }
                }
            });

            Assert.IsType<ObjectResult>(result);
            await productsRepository.Received(1).GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>());
            await priceService.Received(1).GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
            Assert.NotNull(savedItems);

            var resolved = savedItems.Single(item => item.ProductSku == "RESOLVED");
            Assert.Equal(12m, resolved.UnitPrice);
            Assert.Equal(24m, resolved.Price);
            Assert.Equal("EUR", resolved.Currency);
            Assert.Equal("Canonical product name", resolved.ProductName);

            var unresolved = savedItems.Single(item => item.ProductSku == "UNKNOWN");
            Assert.Null(unresolved.UnitPrice);
            Assert.Null(unresolved.Price);
            Assert.Null(unresolved.Currency);

            var pricedProducts = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[1] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("RESOLVED", priceProduct.ProductVariantSku);
            Assert.Equal("No", priceProduct.IsOutlet);

            var priceClient = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[2] as PriceClient;
            Assert.Equal("SUMMER25", priceClient.DiscountCode);
        }

        [Fact]
        public async Task Index_WhenResolvedSkuDoesNotMatchSubmittedProductId_RejectsBeforePricingOrSaving()
        {
            var basketId = Guid.NewGuid();
            var resolvedProductId = Guid.NewGuid();
            var basketRepository = Substitute.For<IBasketRepository>();
            var productsRepository = Substitute.For<IProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var orderLocalizer = Substitute.For<IStringLocalizer<OrderResources>>();

            orderLocalizer[Arg.Any<string>()]
                .Returns(new LocalizedString("BasketPricesCouldNotBeVerified", "Basket prices could not be verified."));

            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<Product>>(new[]
                {
                    new Product { Id = resolvedProductId, Sku = "CHEAP-SKU", Name = "Cheap product", PrimaryProductSku = "PRIMARY" }
                }));

            var productsService = Substitute.For<IProductsService>();
            var productColorsService = Substitute.For<IProductColorsService>();
            var options = Options.Create(new AppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            var httpContext = CreateHttpContext(basketId);

            var controller = new BasketsApiController(
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                orderLocalizer,
                productsRepository,
                priceService,
                productsService,
                productColorsService,
                options,
                Substitute.For<ILogger<BasketsApiController>>(),
                new PriceProductFactory(productsService, productColorsService, options),
                CreatePriceClientResolver(httpContext),
                CreateBasketRepricingService(priceService))
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            await Assert.ThrowsAsync<CustomException>(() => controller.Index(new SaveBasketRequestModel
            {
                Items = new[]
                {
                    new BasketItemRequestModel
                    {
                        ProductId = Guid.NewGuid(),
                        Sku = "CHEAP-SKU",
                        Name = "Expensive product",
                        Quantity = 1
                    }
                }
            }));

            await priceService.DidNotReceive().GetPriceResultsForBasketAsync(
                Arg.Any<DateTime>(),
                Arg.Any<IEnumerable<PriceProduct>>(),
                Arg.Any<PriceClient>());
            await basketRepository.DidNotReceive().SaveAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<Guid?>(),
                Arg.Any<IEnumerable<BasketItem>>(),
                Arg.Any<string>());
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

        private static DefaultHttpContext CreateHttpContext(Guid basketId)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={basketId}";
            context.User = new ClaimsPrincipal(new ClaimsIdentity());
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
    // they now delegate to stays pinned from both ends. The Seller controller resolves its
    // PriceClient from a client id rather than the request principal, so IPriceClientResolver is
    // substituted directly here - the resolver's own null-safety is pinned separately by
    // ClientRepositoryPriceClientResolverTests.
    public class SellerBasketsApiControllerFlowTests
    {
        [Fact]
        public async Task Index_WhenBasketContainsAnUnresolvableSku_SavesThatLineUnpricedAndPricesResolvedLines()
        {
            var basketId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            IEnumerable<SellerBasketItem> savedItems = null;
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IPriceClientResolver>();
            var options = Options.Create(new SellerAppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[]
                {
                    new SellerProduct { Id = productId, Sku = "RESOLVED", Name = "Canonical product name", PrimaryProductSku = "PRIMARY" }
                }));
            productColorsService.ToEnglishAsync(Arg.Any<string>()).Returns(Task.FromResult<string>(null));
            priceService.GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult
                    {
                        Status = PriceLookupStatus.Priced,
                        Price = new Price { CurrentPrice = 12m, CurrencyCode = "EUR" }
                    }
                }));
            priceService.CanSeePrices(Arg.Any<Guid?>()).Returns(true);
            priceClientResolver.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(new PriceClient { Id = call.ArgAt<Guid?>(0), DiscountCode = call.ArgAt<string>(1) }));
            basketRepository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), basketId, Arg.Any<IEnumerable<SellerBasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    savedItems = call.ArgAt<IEnumerable<SellerBasketItem>>(3).ToList();
                    return Task.FromResult(new SellerBasket { Id = basketId, Items = savedItems });
                });

            var controller = new SellerBasketsApiController(
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                productsRepository,
                priceService,
                productsService,
                productColorsService,
                Substitute.For<IStringLocalizer<OrderResources>>(),
                options,
                priceClientResolver,
                Substitute.For<ILogger<SellerBasketsApiController>>(),
                new SellerPriceProductFactory(productsService, productColorsService, options),
                CreateBasketRepricingService(priceService))
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
            };

            var result = await controller.Index(new SellerSaveBasketRequestModel
            {
                Id = basketId,
                ClientId = clientId,
                DiscountCode = "SUMMER25",
                Items = new[]
                {
                    new SellerBasketItemRequestModel { ProductId = productId, Sku = "RESOLVED", Name = "Spoofed product name", Quantity = 2, UnitPrice = 0.01m, Price = 0.02m, Currency = "FAKE" },
                    new SellerBasketItemRequestModel { ProductId = Guid.NewGuid(), Sku = "UNKNOWN", Name = "Unknown", Quantity = 1, UnitPrice = 999m, Price = 999m, Currency = "USD" }
                }
            });

            Assert.IsType<ObjectResult>(result);
            await productsRepository.Received(1).GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>());
            await priceService.Received(1).GetPriceResultsForBasketAsync(Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>());
            await priceClientResolver.Received(1).ResolveAsync(clientId, "SUMMER25", Arg.Any<string>());
            Assert.NotNull(savedItems);

            var resolved = savedItems.Single(item => item.ProductSku == "RESOLVED");
            Assert.Equal(12m, resolved.UnitPrice);
            Assert.Equal(24m, resolved.Price);
            Assert.Equal("EUR", resolved.Currency);
            Assert.Equal("Canonical product name", resolved.ProductName);

            var unresolved = savedItems.Single(item => item.ProductSku == "UNKNOWN");
            Assert.Null(unresolved.UnitPrice);
            Assert.Null(unresolved.Price);
            Assert.Null(unresolved.Currency);

            var pricedProducts = priceService.ReceivedCalls()
                .Single(call => call.GetMethodInfo().Name == nameof(IPriceService.GetPriceResultsForBasketAsync))
                .GetArguments()[1] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("RESOLVED", priceProduct.ProductVariantSku);
            Assert.Equal("No", priceProduct.IsOutlet);
        }

        [Fact]
        public async Task Index_WhenResolvedSkuDoesNotMatchSubmittedProductId_RejectsBeforePricingOrSaving()
        {
            var basketId = Guid.NewGuid();
            var resolvedProductId = Guid.NewGuid();
            var basketRepository = Substitute.For<SellerIBasketRepository>();
            var productsRepository = Substitute.For<SellerIProductsRepository>();
            var priceService = Substitute.For<IPriceService>();
            var orderLocalizer = Substitute.For<IStringLocalizer<OrderResources>>();

            orderLocalizer[Arg.Any<string>()]
                .Returns(new LocalizedString("BasketPricesCouldNotBeVerified", "Basket prices could not be verified."));

            productsRepository.GetProductsBySkusAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IEnumerable<SellerProduct>>(new[]
                {
                    new SellerProduct { Id = resolvedProductId, Sku = "CHEAP-SKU", Name = "Cheap product", PrimaryProductSku = "PRIMARY" }
                }));

            var productsService = Substitute.For<SellerIProductsService>();
            var productColorsService = Substitute.For<SellerIProductColorsService>();
            var priceClientResolver = Substitute.For<IPriceClientResolver>();
            var options = Options.Create(new SellerAppSettings
            {
                GrulaAccessToken = "test-token",
                GrulaEnvironmentId = Guid.NewGuid().ToString()
            });

            var controller = new SellerBasketsApiController(
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Substitute.For<IMediaService>(),
                productsRepository,
                priceService,
                productsService,
                productColorsService,
                orderLocalizer,
                options,
                priceClientResolver,
                Substitute.For<ILogger<SellerBasketsApiController>>(),
                new SellerPriceProductFactory(productsService, productColorsService, options),
                CreateBasketRepricingService(priceService))
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
            };

            await Assert.ThrowsAsync<CustomException>(() => controller.Index(new SellerSaveBasketRequestModel
            {
                Id = basketId,
                ClientId = Guid.NewGuid(),
                Items = new[]
                {
                    new SellerBasketItemRequestModel
                    {
                        ProductId = Guid.NewGuid(),
                        Sku = "CHEAP-SKU",
                        Name = "Expensive product",
                        Quantity = 1
                    }
                }
            }));

            await priceService.DidNotReceive().GetPriceResultsForBasketAsync(
                Arg.Any<DateTime>(),
                Arg.Any<IEnumerable<PriceProduct>>(),
                Arg.Any<PriceClient>());
            await basketRepository.DidNotReceive().SaveAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<Guid?>(),
                Arg.Any<IEnumerable<SellerBasketItem>>(),
                Arg.Any<string>());
        }


        // The controllers delegate the align/apply spine to BasketRepricingService, so these flow
        // tests drive the real one over a substituted IPriceService rather than stubbing it out.
        private static IBasketRepricingService CreateBasketRepricingService(IPriceService priceService)
        {
            return new BasketRepricingService(priceService, Substitute.For<ILogger<BasketRepricingService>>());
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };
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