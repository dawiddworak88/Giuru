using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Products.DomainModels;
using Foundation.Extensions.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    internal static class OrderBasketUploadHelper
    {
        private readonly record struct BasketLineKey(
            Guid? ProductId,
            string ProductSku,
            bool IsOutlet,
            string ExternalReference,
            string MoreInfo);

        internal static string NormalizeKeyPart(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static BasketLineKey CreateKey(BasketItem item)
        {
            return new BasketLineKey(
                item.ProductId,
                item.ProductId is null ? item.ProductSku : null,
                item.OutletQuantity > 0,
                NormalizeKeyPart(item.ExternalReference),
                NormalizeKeyPart(item.MoreInfo));
        }

        internal static IReadOnlyList<OrderFileLine> GroupImportedLines(IEnumerable<OrderFileLine> lines)
        {
            var groupedLines = new List<OrderFileLine>();
            var linesByKey = new Dictionary<(string Sku, string ExternalReference, string MoreInfo), OrderFileLine>();

            foreach (var line in lines.OrEmptyIfNull())
            {
                if (string.IsNullOrWhiteSpace(line.Sku))
                {
                    groupedLines.Add(line);
                    continue;
                }

                var externalReference = NormalizeKeyPart(line.ExternalReference);
                var moreInfo = NormalizeKeyPart(line.MoreInfo);
                var key = (line.Sku, externalReference, moreInfo);

                if (linesByKey.TryGetValue(key, out var existingLine))
                {
                    existingLine.Quantity += line.Quantity;
                    continue;
                }

                var normalizedLine = new OrderFileLine
                {
                    Sku = line.Sku,
                    Quantity = line.Quantity,
                    ExternalReference = externalReference,
                    MoreInfo = moreInfo
                };

                linesByKey[key] = normalizedLine;
                groupedLines.Add(normalizedLine);
            }

            return groupedLines;
        }

        internal static IReadOnlyList<BasketItem> Merge(
            IEnumerable<BasketItem> existingItems,
            IEnumerable<BasketItem> importedItems)
        {
            var mergedItems = new List<BasketItem>();
            var itemsByKey = new Dictionary<BasketLineKey, BasketItem>();

            foreach (var item in existingItems.OrEmptyIfNull().Concat(importedItems.OrEmptyIfNull()))
            {
                var key = CreateKey(item);

                if (itemsByKey.TryGetValue(key, out var existingItem))
                {
                    var mergedIndex = mergedItems.IndexOf(existingItem);
                    var mergedItem = MergeLines(existingItem, item);

                    mergedItems[mergedIndex] = mergedItem;
                    itemsByKey[key] = mergedItem;
                    continue;
                }

                itemsByKey[key] = item;
                mergedItems.Add(item);
            }

            return mergedItems;
        }

        private static BasketItem MergeLines(BasketItem target, BasketItem addition)
        {
            var quantity = target.Quantity + addition.Quantity;
            var stockQuantity = target.StockQuantity + addition.StockQuantity;
            var outletQuantity = target.OutletQuantity + addition.OutletQuantity;

            // Merged lines are deliberately unpriced; the caller must reprice the final set before saving.

            return new BasketItem
            {
                ProductId = target.ProductId,
                ProductSku = target.ProductSku,
                ProductName = target.ProductName ?? addition.ProductName,
                PictureUrl = target.PictureUrl ?? addition.PictureUrl,
                Quantity = quantity,
                StockQuantity = stockQuantity,
                OutletQuantity = outletQuantity,
                ExternalReference = target.ExternalReference,
                MoreInfo = target.MoreInfo,
                UnitPrice = null,
                Price = null,
                Currency = null,
                ExpectedLeadTime = target.ExpectedLeadTime ?? addition.ExpectedLeadTime
            };
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

    }
}