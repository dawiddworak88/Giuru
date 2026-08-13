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
using Buyer.Web.Shared.DomainModels.Prices;
using Buyer.Web.Shared.Repositories.Inventory;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Services.Prices;
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

namespace Giuru.UnitTests.Orders.Baskets
{
    public class OrderFileApiControllerFlowTests
    {
        [Fact]
        public async Task Index_WhenMergedLineHasAnUnpricedPricingResult_DoesNotRetainTheExistingPrice()
        {
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            IEnumerable<BasketItem> savedItems = null;
            var existingBasket = new Basket
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
            var productColorsService = Substitute.For<IProductColorsService>();

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
            priceService.GetPriceResultsForBasketAsync("token", Arg.Any<DateTime>(), Arg.Any<IEnumerable<PriceProduct>>(), Arg.Any<PriceClient>())
                .Returns(Task.FromResult<IReadOnlyList<PriceLookupResult>>(new[]
                {
                    new PriceLookupResult { Status = PriceLookupStatus.InvalidPriceDrivers }
                }));
            basketRepository.SaveAsync("token", Arg.Any<string>(), basketId, Arg.Any<IEnumerable<BasketItem>>(), Arg.Any<string>())
                .Returns(call =>
                {
                    savedItems = call.ArgAt<IEnumerable<BasketItem>>(3).ToList();
                    return Task.FromResult(new Basket { Id = basketId, Items = savedItems });
                });

            var controller = new OrderFileApiController(
                orderFileService,
                productsRepository,
                basketRepository,
                Substitute.For<LinkGenerator>(),
                Options.Create(new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() }),
                Substitute.For<IMediaService>(),
                Substitute.For<IMediaItemsRepository>(),
                Substitute.For<IOrdersRepository>(),
                inventoryRepository,
                Substitute.For<ILogger<OrderFileApiController>>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                priceService,
                Substitute.For<IProductsService>(),
                productColorsService)
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext(basketId) }
            };

            var result = await controller.Index(new UploadMediaRequestModel { File = Substitute.For<IFormFile>() });

            Assert.IsType<ObjectResult>(result);
            await priceService.Received(1).GetPriceResultsForBasketAsync(
                "test-token",
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
                .GetArguments()[2] as IEnumerable<PriceProduct>;
            var priceProduct = Assert.Single(pricedProducts);
            Assert.Equal("SKU", priceProduct.ProductVariantSku);
        }

        private static DefaultHttpContext CreateHttpContext(Guid basketId)
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = $"{BasketConstants.BasketCookieName}={basketId}";
            context.Features.Set<IFormFeature>(new FormFeature(new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>())));
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
}