using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<InventoryResources> _inventoryLocalizer;

        public BasketService(
            IBasketRepository basketRepository,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            _basketRepository = basketRepository;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _orderLocalizer = orderLocalizer;
            _globalLocalizer = globalLocalizer;
            _inventoryLocalizer = inventoryLocalizer;
        }

        public async Task ValidateStockOutletQuantitiesAsync(string token, string language, Guid? basketId)
        {
            var basket = await _basketRepository.GetBasketByIdAsync(token, language, basketId);

            if (basket?.Items == null || basket.Items.Any() is false)
            {
                throw new CustomException(_orderLocalizer.GetString("BasketNotFound").Value, (int)HttpStatusCode.NotFound);
            }

            var items = basket.Items.OrEmptyIfNull().ToList();

            if (items.Any(x => x.ProductId.HasValue is false))
            {
                throw new CustomException(_orderLocalizer.GetString("ProductsNotFound"), (int)HttpStatusCode.NotFound);
            }

            await ValidateStockTypeAsync<InventoryItem>(
                token, language, items,
                quantitySelector: x => x.StockQuantity,
                fetchProducts: ids => _inventoryRepository.GetInventoryProductByProductIdsAsync(token, language, ids),
                messageNotFound: _inventoryLocalizer.GetString("InventoryNotFound"),
                messageQuantityError: _orderLocalizer.GetString("StockQuantityError").Value);

            await ValidateStockTypeAsync<OutletItem>(
                token, language, items,
                quantitySelector: x => x.OutletQuantity,
                fetchProducts: ids => _outletRepository.GetOutletProductsByProductsIdAsync(token, language, ids),
                messageNotFound: _inventoryLocalizer.GetString("OutletNotFound"),
                messageQuantityError: _orderLocalizer.GetString("OutletQuantityError").Value);
        }

        private async Task ValidateStockTypeAsync<T>(
            string token,
            string language,
            IEnumerable<DomainModels.BasketItem> basketItems,
            Func<DomainModels.BasketItem, double> quantitySelector,
            Func<IEnumerable<Guid>, Task<IEnumerable<T>>> fetchProducts,
            string messageNotFound,
            string messageQuantityError) where T : StockItem
        {
            var filteredItems = basketItems
                .Where(x => quantitySelector(x) > 0)
                .Select(x => new BasketItem
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductSku = x.ProductSku,
                    Quantity = quantitySelector(x),
                }).ToList();

            if (filteredItems.Any() is false)
            {
                return;
            }

            var stockProducts = (await fetchProducts(filteredItems.Select(x => x.ProductId.Value))).OrEmptyIfNull();

            ValidateQuantities<T>(filteredItems, stockProducts, messageNotFound, messageQuantityError);
        }

        private void ValidateQuantities<T>(IEnumerable<BasketItem> items, IEnumerable<T> stockProducts, string messageNotFound, string messageQuantityError) where T : StockItem
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

        private class BasketItem
        {
            public Guid? ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductSku { get; set; }
            public double Quantity { get; set; }
        }
    }
}
