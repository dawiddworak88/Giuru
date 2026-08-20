using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Shared.DomainModels.Baskets;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Baskets
{
    public class BasketService : IBasketService
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketRepository basketRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IOutletRepository outletRepository;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IPriceService priceService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BasketService(
            LinkGenerator linkGenerator,
            IBasketRepository basketRepository,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IPriceService priceService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
            this.inventoryRepository = inventoryRepository;
            this.outletRepository = outletRepository;
            this.orderLocalizer = orderLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.priceService = priceService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Basket> GetBasketAsync(Guid? basketId, string token, string language)
        {
            var existingBasket = await this.basketRepository.GetBasketById(token, language, basketId);

            if (existingBasket is not null)
            {
                var canSeePrices = this.priceService.CanSeePrices(
                    this.httpContextAccessor.HttpContext?.User.GetClientId());

                return new Basket
                {
                    Id = existingBasket.Id,
                    MoreInfo = existingBasket.MoreInfo,
                    DiscountCode = existingBasket.DiscountCode,
                    Items = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItem
                    {
                        ProductId = x.ProductId,
                        ProductUrl = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        UnitPrice = canSeePrices ? x.UnitPrice : null,
                        Price = canSeePrices ? x.Price : null,
                        Currency = canSeePrices ? x.Currency : null,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        MoreInfo = x.MoreInfo,
                        ExpectedLeadTime = x.ExpectedLeadTime
                    })
                };
            }

            return default;
        }

        public async Task ValidateStockOutletQuantitiesAsync(Guid? basketId, string token, string language)
        {
            var basket = await this.basketRepository.GetBasketById(token, language, basketId);

            if (basket is not null && basket.Items.Any())
            {
                if (basket.Items.Any(x => x.StockQuantity > 0))
                {
                    var stockItems = basket.Items.Where(x => x.StockQuantity > 0);

                    var inventoryProducts = await this.inventoryRepository.GetAvailbleProductsByProductIdsAsync(token, language, stockItems.Select(x => x.ProductId.Value));

                    foreach (var item in stockItems)
                    {
                        var inventoryProductAvailableQuantitiy = inventoryProducts.Where(x => x.ProductId == item.ProductId).Sum(x => x.AvailableQuantity);
                        var itemStockQuantity = stockItems.Where(x => x.ProductId == item.ProductId).Sum(x => x.StockQuantity);

                        if (itemStockQuantity > inventoryProductAvailableQuantitiy)
                        {
                            throw new CustomException($"{this.orderLocalizer.GetString("StockQuantityError").Value} {item.ProductName} ({item.ProductSku}) {this.globalLocalizer.GetString("InBasket")} {itemStockQuantity} {this.globalLocalizer.GetString("MaximalLabel")} {inventoryProductAvailableQuantitiy}", (int)HttpStatusCode.Conflict);
                        }
                    }
                }

                if (basket.Items.Any(x => x.OutletQuantity > 0))
                {
                    var outletItems = basket.Items.Where(x => x.OutletQuantity > 0);

                    var outletProducts = await this.outletRepository.GetOutletProductsByProductsIdAsync(token, language, outletItems.Select(x => x.ProductId.Value));

                    foreach (var item in outletItems)
                    {
                        var outletProductAvailableQuantity = outletProducts.Where(x => x.ProductId == item.ProductId).Sum(x => x.AvailableQuantity);
                        var itemOutletQuantity = outletItems.Where(x => x.ProductId == item.ProductId).Sum(x => x.OutletQuantity);

                        if (itemOutletQuantity > outletProductAvailableQuantity)
                        {
                            throw new CustomException($"{this.orderLocalizer.GetString("OutletQuantityError").Value} {item.ProductName} ({item.ProductSku}) {this.globalLocalizer.GetString("InBasket")} {itemOutletQuantity} {this.globalLocalizer.GetString("MaximalLabel")} {outletProductAvailableQuantity}", (int)HttpStatusCode.Conflict);
                        }
                    }
                }
            }
        }
    }
}
