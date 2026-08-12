using System;
using System.Collections.Generic;
using System.Linq;
using BuyerBasketItem = Buyer.Web.Areas.Orders.DomainModels.BasketItem;
using BuyerOrderBasketUploadHelper = Buyer.Web.Areas.Orders.Services.Basket.OrderBasketUploadHelper;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerOrderBasketUploadHelper = Seller.Web.Areas.Orders.Services.Basket.OrderBasketUploadHelper;

namespace Giuru.UnitTests.Orders.Baskets
{
    public abstract class OrderBasketUploadHelperTests<TBasketItem>
        where TBasketItem : class
    {
        protected abstract TBasketItem CreateItem(
            Guid productId,
            double quantity,
            double stockQuantity,
            double outletQuantity,
            string reference,
            string notes,
            decimal unitPrice,
            decimal price,
            string currency,
            DateOnly expectedLeadTime);

        protected abstract IReadOnlyList<TBasketItem> Append(
            IEnumerable<TBasketItem> existingItems,
            IEnumerable<TBasketItem> importedItems);

        protected abstract void DeductExistingStock(
            IDictionary<Guid, double> availableStockByProductId,
            IEnumerable<TBasketItem> existingItems);

        protected abstract Guid? ProductId(TBasketItem item);
        protected abstract double TotalQuantity(TBasketItem item);

        [Fact]
        public void Append_RepeatedUploads_PreservesEveryPreviouslyPersistedLineAndItsMetadata()
        {
            var manualItem = CreateItem(
                Guid.NewGuid(), 1, 2, 3, "manual-ref", "manual-notes", 12.50m, 75m, "EUR", new DateOnly(2026, 9, 1));
            var firstImport = CreateItem(
                Guid.NewGuid(), 4, 0, 0, "first-ref", "first-notes", 20m, 80m, "EUR", new DateOnly(2026, 10, 1));
            var secondImport = CreateItem(
                Guid.NewGuid(), 5, 1, 0, "second-ref", "second-notes", 30m, 180m, "EUR", new DateOnly(2026, 11, 1));

            var afterFirstUpload = Append(new[] { manualItem }, new[] { firstImport });
            var afterSecondUpload = Append(afterFirstUpload, new[] { secondImport });

            Assert.Equal(3, afterSecondUpload.Count);
            Assert.Same(manualItem, afterSecondUpload[0]);
            Assert.Same(firstImport, afterSecondUpload[1]);
            Assert.Same(secondImport, afterSecondUpload[2]);
        }

        [Fact]
        public void Append_DuplicateProduct_KeepsBothLinesAndDoesNotDoubleCountEitherQuantity()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 3, 0, "manual-ref", "manual-notes", 10m, 50m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 4, 1, 0, "upload-ref", "upload-notes", 10m, 50m, "EUR", new DateOnly(2026, 9, 1));

            var result = Append(new[] { existingItem }, new[] { importedItem });

            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(productId, ProductId(item)));
            Assert.Equal(10, result.Sum(TotalQuantity));
        }

        [Fact]
        public void DeductExistingStock_SubtractsAllExistingAllocationsBeforeAnImportAllocatesStock()
        {
            var matchingProductId = Guid.NewGuid();
            var otherProductId = Guid.NewGuid();
            var availableStock = new Dictionary<Guid, double>
            {
                [matchingProductId] = 7,
                [otherProductId] = 9
            };
            var existingItems = new[]
            {
                CreateItem(matchingProductId, 0, 2, 0, "one", null, 10m, 20m, "EUR", new DateOnly(2026, 9, 1)),
                CreateItem(matchingProductId, 0, 3, 4, "two", null, 10m, 70m, "EUR", new DateOnly(2026, 9, 1))
            };

            DeductExistingStock(availableStock, existingItems);

            Assert.Equal(2, availableStock[matchingProductId]);
            Assert.Equal(9, availableStock[otherProductId]);
        }
    }

    public sealed class BuyerOrderBasketUploadHelperTests : OrderBasketUploadHelperTests<BuyerBasketItem>
    {
        protected override BuyerBasketItem CreateItem(
            Guid productId, double quantity, double stockQuantity, double outletQuantity,
            string reference, string notes, decimal unitPrice, decimal price, string currency, DateOnly expectedLeadTime)
        {
            return new BuyerBasketItem
            {
                ProductId = productId,
                ProductSku = $"SKU-{productId}",
                ProductName = $"Product {productId}",
                PictureUrl = $"/{productId}.jpg",
                Quantity = quantity,
                StockQuantity = stockQuantity,
                OutletQuantity = outletQuantity,
                ExternalReference = reference,
                MoreInfo = notes,
                UnitPrice = unitPrice,
                Price = price,
                Currency = currency,
                ExpectedLeadTime = expectedLeadTime
            };
        }

        protected override IReadOnlyList<BuyerBasketItem> Append(IEnumerable<BuyerBasketItem> existingItems, IEnumerable<BuyerBasketItem> importedItems)
            => BuyerOrderBasketUploadHelper.Append(existingItems, importedItems);

        protected override void DeductExistingStock(IDictionary<Guid, double> availableStockByProductId, IEnumerable<BuyerBasketItem> existingItems)
            => BuyerOrderBasketUploadHelper.DeductExistingStock(availableStockByProductId, existingItems);

        protected override Guid? ProductId(BuyerBasketItem item) => item.ProductId;
        protected override double TotalQuantity(BuyerBasketItem item) => item.Quantity + item.StockQuantity + item.OutletQuantity;
    }

    public sealed class SellerOrderBasketUploadHelperTests : OrderBasketUploadHelperTests<SellerBasketItem>
    {
        protected override SellerBasketItem CreateItem(
            Guid productId, double quantity, double stockQuantity, double outletQuantity,
            string reference, string notes, decimal unitPrice, decimal price, string currency, DateOnly expectedLeadTime)
        {
            return new SellerBasketItem
            {
                ProductId = productId,
                ProductSku = $"SKU-{productId}",
                ProductName = $"Product {productId}",
                PictureUrl = $"/{productId}.jpg",
                Quantity = quantity,
                StockQuantity = stockQuantity,
                OutletQuantity = outletQuantity,
                ExternalReference = reference,
                MoreInfo = notes,
                UnitPrice = unitPrice,
                Price = price,
                Currency = currency,
                ExpectedLeadTime = expectedLeadTime
            };
        }

        protected override IReadOnlyList<SellerBasketItem> Append(IEnumerable<SellerBasketItem> existingItems, IEnumerable<SellerBasketItem> importedItems)
            => SellerOrderBasketUploadHelper.Append(existingItems, importedItems);

        protected override void DeductExistingStock(IDictionary<Guid, double> availableStockByProductId, IEnumerable<SellerBasketItem> existingItems)
            => SellerOrderBasketUploadHelper.DeductExistingStock(availableStockByProductId, existingItems);

        protected override Guid? ProductId(SellerBasketItem item) => item.ProductId;
        protected override double TotalQuantity(SellerBasketItem item) => item.Quantity + item.StockQuantity + item.OutletQuantity;
    }
}