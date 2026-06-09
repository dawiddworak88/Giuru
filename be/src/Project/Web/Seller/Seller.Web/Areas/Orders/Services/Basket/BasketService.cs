using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using System;
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

        public BasketService(
            IBasketRepository basketRepository,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _basketRepository = basketRepository;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
            _orderLocalizer = orderLocalizer;
            _globalLocalizer = globalLocalizer;
        }

        public async Task ValidateStockOutletQuantitiesAsync(string token, string language, Guid? basketId)
        {
            var basket = await _basketRepository.GetBasketByIdAsync(token, language, basketId);

            if (basket is null) throw new CustomException(_orderLocalizer.GetString("BasketNotFound"), (int)HttpStatusCode.NotFound);

            if (basket.Items.Any())
            {
                if (basket.Items.Any(x => x.StockQuantity > 0))
                {
                    var stockItems = basket.Items.Where(x => x.StockQuantity > 0).ToList();

                    var inventoryProducts = await _inventoryRepository.GetInventoryProductByProductIdsAsync(token, language, stockItems.Select(x => x.ProductId.Value));

                    var inventoryAvailableByProduct = inventoryProducts
                        .GroupBy(x => x.ProductId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.AvailableQuantity));

                    var stockQuantityByProduct = stockItems
                        .GroupBy(x => x.ProductId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.StockQuantity));

                    foreach (var item in stockItems)
                    {
                        inventoryAvailableByProduct.TryGetValue((Guid)item.ProductId, out var inventoryProductAvailableQuantity);
                        stockQuantityByProduct.TryGetValue(item.ProductId, out var itemStockQuantity);

                        if (itemStockQuantity > inventoryProductAvailableQuantity)
                        {
                            throw new CustomException($"{_orderLocalizer.GetString("StockQuantityError").Value} {item.ProductName} ({item.ProductSku}) {_globalLocalizer.GetString("InBasket")} {itemStockQuantity} {_globalLocalizer.GetString("MaximalLabel")} {inventoryProductAvailableQuantity}", (int)HttpStatusCode.Conflict);
                        }
                    }
                }

                if (basket.Items.Any(x => x.OutletQuantity > 0))
                {
                    var outletItems = basket.Items.Where(x => x.OutletQuantity > 0).ToList();

                    var outletProducts = await _outletRepository.GetOutletProductsByProductsIdAsync(token, language, outletItems.Select(x => x.ProductId.Value));

                    var outletAvailableByProduct = outletProducts
                        .GroupBy(x => x.ProductId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.AvailableQuantity));

                    var outletQuantityByProduct = outletItems
                        .GroupBy(x => x.ProductId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.OutletQuantity));

                    foreach (var item in outletItems)
                    {
                        outletAvailableByProduct.TryGetValue((Guid)item.ProductId, out var outletProductAvailableQuantity);
                        outletQuantityByProduct.TryGetValue(item.ProductId, out var itemOutletQuantity);

                        if (itemOutletQuantity > outletProductAvailableQuantity)
                        {
                            throw new CustomException($"{_orderLocalizer.GetString("OutletQuantityError").Value} {item.ProductName} ({item.ProductSku}) {_globalLocalizer.GetString("InBasket")} {itemOutletQuantity} {_globalLocalizer.GetString("MaximalLabel")} {outletProductAvailableQuantity}", (int)HttpStatusCode.Conflict);
                        }
                    }
                }
            }
        }
    }
}
