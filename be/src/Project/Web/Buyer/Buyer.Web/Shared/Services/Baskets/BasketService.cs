using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Shared.DomainModels.Baskets;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
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

        public BasketService(
            LinkGenerator linkGenerator,
            IBasketRepository basketRepository,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
            this.inventoryRepository = inventoryRepository;
            this.outletRepository = outletRepository;
            this.orderLocalizer = orderLocalizer;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<IEnumerable<BasketItem>> GetBasketAsync(Guid? basketId, string token, string language)
        {
            var existingBasket = await this.basketRepository.GetBasketById(token, language, basketId);

            if (existingBasket is not null)
            {
                var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                if (productIds.OrEmptyIfNull().Any())
                {
                    var basketItems = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItem
                    {
                        ProductId = x.ProductId,
                        ProductUrl = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        MoreInfo = x.MoreInfo
                    });

                    return basketItems;
                }
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
                        var inventoryProduct = inventoryProducts.FirstOrDefault(x => x.ProductId == item.ProductId);

                        if (inventoryProduct is not null)
                        {
                            var itemStockQuantity = stockItems.Where(x => x.ProductId == item.ProductId).Sum(x => x.StockQuantity);

                            if (itemStockQuantity > inventoryProduct.AvailableQuantity)
                            {
                                throw new CustomException($"{this.orderLocalizer.GetString("StockQuantityError").Value} {item.ProductName} ({item.ProductSku}) {this.globalLocalizer.GetString("InBasket")} {itemStockQuantity} {this.globalLocalizer.GetString("MaximalLabel")} {inventoryProduct.AvailableQuantity}", (int)HttpStatusCode.Conflict);
                            }
                        }
                    }
                }

                if (basket.Items.Any(x => x.OutletQuantity > 0))
                {
                    var outletItems = basket.Items.Where(x => x.OutletQuantity > 0);

                    var outletProducts = await this.outletRepository.GetOutletProductsByProductsIdAsync(token, language, outletItems.Select(x => x.ProductId.Value));

                    foreach (var item in outletItems)
                    {
                        var outletProduct = outletProducts.FirstOrDefault(x => x.ProductId == item.ProductId);

                        if (outletProduct is not null)
                        {
                            var itemOutletQuantity = outletItems.Where(x => x.ProductId == item.ProductId).Sum(x => x.OutletQuantity);

                            if (itemOutletQuantity > outletProduct.AvailableQuantity)
                            {
                                throw new CustomException($"{this.orderLocalizer.GetString("OutletQuantityError").Value} {item.ProductName} ({item.ProductSku}) {this.globalLocalizer.GetString("InBasket")} {itemOutletQuantity} {this.globalLocalizer.GetString("MaximalLabel")} {outletProduct.AvailableQuantity}", (int)HttpStatusCode.Conflict);
                            }
                        }
                    }
                }
            }
        }
    }
}
