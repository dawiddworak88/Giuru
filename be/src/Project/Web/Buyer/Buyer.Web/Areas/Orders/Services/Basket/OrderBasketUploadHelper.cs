using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buyer.Web.Areas.Orders.Services.Basket
{
    internal static class OrderBasketUploadHelper
    {
        internal static IReadOnlyList<BasketItem> Append(
            IEnumerable<BasketItem> existingItems,
            IEnumerable<BasketItem> importedItems)
        {
            return existingItems
                .OrEmptyIfNull()
                .Concat(importedItems.OrEmptyIfNull())
                .ToList();
        }

        internal static IReadOnlyDictionary<string, Product> CreateProductLookup(IEnumerable<Product> products)
        {
            return products
                .OrEmptyIfNull()
                .Where(x => !string.IsNullOrWhiteSpace(x.Sku))
                .GroupBy(x => x.Sku)
                .ToDictionary(x => x.Key, x => x.First());
        }

        internal static void DeductExistingStock(
            IDictionary<Guid, double> availableStockByProductId,
            IEnumerable<BasketItem> existingItems)
        {
            if (availableStockByProductId is null)
            {
                return;
            }

            var existingStockByProductId = existingItems
                .OrEmptyIfNull()
                .Where(x => x.ProductId.HasValue && x.StockQuantity > 0)
                .GroupBy(x => x.ProductId.Value)
                .ToDictionary(x => x.Key, x => x.Sum(item => item.StockQuantity));

            foreach (var existingStock in existingStockByProductId)
            {
                if (availableStockByProductId.TryGetValue(existingStock.Key, out var availableStock))
                {
                    availableStockByProductId[existingStock.Key] = Math.Max(0, availableStock - existingStock.Value);
                }
            }
        }

        private static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> items)
        {
            return items ?? Enumerable.Empty<T>();
        }
    }
}