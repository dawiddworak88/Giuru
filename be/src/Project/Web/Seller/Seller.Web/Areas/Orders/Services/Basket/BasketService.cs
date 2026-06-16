using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> _inventoryLocalizer;

        public BasketService(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            _orderLocalizer = orderLocalizer;
            _globalLocalizer = globalLocalizer;
            _inventoryLocalizer = inventoryLocalizer;
        }

        public void ValidateStockOutletQuantities(IEnumerable<BasketItem> items, IEnumerable<InventoryItem> inventoryItems, IEnumerable<OutletItem> outletItems)
        {
            if (items.OrEmptyIfNull().Any() is false)
            {
                throw new CustomException(_orderLocalizer.GetString("BasketNotFound").Value, (int)HttpStatusCode.NotFound);
            }

            if (inventoryItems.Any())
            {
                ValidateStockType<InventoryItem>(
                    items,
                    quantitySelector: x => x.StockQuantity,
                    stockProducts: inventoryItems,
                    messageNotFound: _inventoryLocalizer.GetString("InventoryNotFound"),
                    messageQuantityError: _orderLocalizer.GetString("StockQuantityError").Value);
            }
            
            if (outletItems.Any())
            {
                ValidateStockType<OutletItem>(
                    items,
                    quantitySelector: x => x.OutletQuantity,
                    stockProducts: outletItems,
                    messageNotFound: _inventoryLocalizer.GetString("OutletNotFound"),
                    messageQuantityError: _orderLocalizer.GetString("OutletQuantityError").Value);
            }
        }

        private void ValidateStockType<T>(
            IEnumerable<BasketItem> basketItems,
            Func<BasketItem, double> quantitySelector,
            IEnumerable<T> stockProducts,
            string messageNotFound,
            string messageQuantityError) where T : StockItem
        {
            var filteredItems = basketItems
                .Where(x => quantitySelector(x) > 0)
                .Select(x => new BasketValidationItem(
                    x.ProductId,
                    x.ProductName,
                    x.ProductSku,
                    quantitySelector(x))
                ).ToList();

            if (filteredItems.Any() is false)
            {
                return;
            }

            ValidateQuantities<T>(filteredItems, stockProducts, messageNotFound, messageQuantityError);
        }

        private void ValidateQuantities<T>(IEnumerable<BasketValidationItem> items, IEnumerable<T> stockProducts, string messageNotFound, string messageQuantityError) where T : StockItem
        {
            var stockItemsAvailableByProduct = stockProducts
                .GroupBy(x => x.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.AvailableQuantity));

            var itemsByProduct = items
                .GroupBy(x => x.ProductId.Value)
                .ToDictionary(g => g.Key, g => (
                    Quantity: g.Sum(x => x.Quantity),
                    g.First().ProductName,
                    g.First().ProductSku
                ));

            foreach (var (productId, itemData) in itemsByProduct)
            {
                if (!stockItemsAvailableByProduct.TryGetValue(productId, out var stockItemAvailableQuantity))
                {
                    throw new CustomException(messageNotFound, (int)HttpStatusCode.NotFound);
                }

                if (itemData.Quantity > stockItemAvailableQuantity)
                {
                    throw new CustomException($"{messageQuantityError} {itemData.ProductName} ({itemData.ProductSku}) {_globalLocalizer.GetString("InBasket")} {itemData.Quantity} {_globalLocalizer.GetString("MaximalLabel")} {stockItemAvailableQuantity}", (int)HttpStatusCode.Conflict);
                }
            }
        }

        private sealed record BasketValidationItem(
            Guid? ProductId,
            string ProductName,
            string ProductSku,
            double Quantity);
    }
}
