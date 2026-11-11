using System;
using System.Collections.Generic;
using System.Linq;
using BuyerBasketItem = Buyer.Web.Areas.Orders.DomainModels.BasketItem;
using BuyerOrderBasketUploadHelper = Buyer.Web.Areas.Orders.Services.Basket.OrderBasketUploadHelper;
using BuyerOrderFileLine = Buyer.Web.Areas.Orders.DomainModels.OrderFileLine;
using BuyerProduct = Buyer.Web.Areas.Products.DomainModels.Product;
using SellerBasketItem = Seller.Web.Areas.Orders.DomainModels.BasketItem;
using SellerOrderBasketUploadHelper = Seller.Web.Areas.Orders.Services.Basket.OrderBasketUploadHelper;
using SellerOrderFileLine = Seller.Web.Areas.Orders.DomainModels.OrderFileLine;
using SellerProduct = Seller.Web.Areas.Products.DomainModels.Product;

namespace Giuru.UnitTests.Orders.Baskets
{
    public abstract class OrderBasketUploadHelperTests<TBasketItem, TProduct, TOrderFileLine>
        where TBasketItem : class
        where TProduct : class
        where TOrderFileLine : class
    {
        protected abstract TBasketItem CreateItem(
            Guid productId,
            double quantity,
            double stockQuantity,
            double outletQuantity,
            string reference,
            string notes,
            decimal? unitPrice,
            decimal? price,
            string currency,
            DateOnly expectedLeadTime);

        protected abstract IReadOnlyList<TBasketItem> Merge(
            IEnumerable<TBasketItem> existingItems,
            IEnumerable<TBasketItem> importedItems);

        protected abstract void DeductExistingStock(
            IDictionary<Guid, double> availableStockByProductId,
            IEnumerable<TBasketItem> existingItems);

        protected abstract TProduct CreateProduct(string sku);
        protected abstract IReadOnlyDictionary<string, TProduct> CreateProductLookup(IEnumerable<TProduct> products);

        protected abstract Guid? ProductId(TBasketItem item);
        protected abstract double TotalQuantity(TBasketItem item);
        protected abstract double OutletQuantity(TBasketItem item);
        protected abstract decimal? UnitPrice(TBasketItem item);
        protected abstract decimal? Price(TBasketItem item);
        protected abstract string Currency(TBasketItem item);

        protected abstract TOrderFileLine CreateOrderFileLine(string sku, double quantity, string reference, string moreInfo);
        protected abstract IReadOnlyList<TOrderFileLine> GroupImportedLines(IEnumerable<TOrderFileLine> lines);
        protected abstract string Sku(TOrderFileLine line);
        protected abstract double Quantity(TOrderFileLine line);
        protected abstract string ExternalReference(TOrderFileLine line);

        [Fact]
        public void CreateProductLookup_DuplicateSku_RetainsTheFirstProduct()
        {
            var firstProduct = CreateProduct("SKU-1");
            var duplicateProduct = CreateProduct("SKU-1");

            var lookup = CreateProductLookup(new[] { firstProduct, duplicateProduct });

            Assert.Single(lookup);
            Assert.Same(firstProduct, lookup["SKU-1"]);
        }

        [Fact]
        public void CreateProductLookup_DistinctSkus_RetainsEachProduct()
        {
            var firstProduct = CreateProduct("SKU-1");
            var secondProduct = CreateProduct("SKU-2");

            var lookup = CreateProductLookup(new[] { firstProduct, secondProduct });

            Assert.Equal(2, lookup.Count);
            Assert.Same(firstProduct, lookup["SKU-1"]);
            Assert.Same(secondProduct, lookup["SKU-2"]);
        }

        [Fact]
        public void CreateProductLookup_UnusableSkus_ExcludesThem()
        {
            var validProduct = CreateProduct("SKU-1");

            var lookup = CreateProductLookup(new[]
            {
                CreateProduct(null),
                CreateProduct(string.Empty),
                CreateProduct("   "),
                validProduct
            });

            Assert.Single(lookup);
            Assert.Same(validProduct, lookup["SKU-1"]);
        }

        [Fact]
        public void CreateProductLookup_NullCollection_ReturnsEmptyLookup()
        {
            var lookup = CreateProductLookup(null);

            Assert.Empty(lookup);
        }

        [Fact]
        public void CreateProductLookup_DifferentlyCasedSkus_RemainSeparate()
        {
            var lowerCaseProduct = CreateProduct("sku");
            var upperCaseProduct = CreateProduct("SKU");

            var lookup = CreateProductLookup(new[] { lowerCaseProduct, upperCaseProduct });

            Assert.Equal(2, lookup.Count);
            Assert.Same(lowerCaseProduct, lookup["sku"]);
            Assert.Same(upperCaseProduct, lookup["SKU"]);
        }

        [Fact]
        public void Append_RepeatedUploads_PreservesEveryPreviouslyPersistedLineAndItsMetadata()
        {
            var manualItem = CreateItem(
                Guid.NewGuid(), 1, 2, 3, "manual-ref", "manual-notes", 12.50m, 75m, "EUR", new DateOnly(2026, 9, 1));
            var firstImport = CreateItem(
                Guid.NewGuid(), 4, 0, 0, "first-ref", "first-notes", 20m, 80m, "EUR", new DateOnly(2026, 10, 1));
            var secondImport = CreateItem(
                Guid.NewGuid(), 5, 1, 0, "second-ref", "second-notes", 30m, 180m, "EUR", new DateOnly(2026, 11, 1));

            var afterFirstUpload = Merge(new[] { manualItem }, new[] { firstImport });
            var afterSecondUpload = Merge(afterFirstUpload, new[] { secondImport });

            Assert.Equal(3, afterSecondUpload.Count);
            Assert.Same(manualItem, afterSecondUpload[0]);
            Assert.Same(firstImport, afterSecondUpload[1]);
            Assert.Same(secondImport, afterSecondUpload[2]);
        }

        [Fact]
        public void Merge_DuplicateProductWithDifferentReferences_KeepsBothLines()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 3, 0, "manual-ref", "manual-notes", 10m, 50m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 4, 1, 0, "upload-ref", "upload-notes", 10m, 50m, "EUR", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(productId, ProductId(item)));
            Assert.Equal(10, result.Sum(TotalQuantity));
        }

        [Fact]
        public void Merge_ImportedSkuAlreadyInBasket_ProducesOneLineWithSummedQuantities()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 0, 0, "ref", "notes", 10m, 20m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 3, 1, 0, "ref", "notes", 10m, 40m, "EUR", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Single(result);
            Assert.Equal(productId, ProductId(result[0]));
            Assert.Equal(6, TotalQuantity(result[0]));
        }

        [Fact]
        public void Merge_ImportedSkuWithBlankAndNullReference_StillGroups()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 0, 0, string.Empty, "notes", 10m, 20m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 3, 0, 0, null, "notes", 10m, 30m, "EUR", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Single(result);
            Assert.Equal(5, TotalQuantity(result[0]));
        }

        [Fact]
        public void Merge_ImportedLine_DoesNotMergeIntoAnOutletLine()
        {
            var productId = Guid.NewGuid();
            var existingOutletItem = CreateItem(
                productId, 0, 0, 3, "ref", "notes", 10m, 30m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 2, 0, 0, "ref", "notes", 10m, 20m, "EUR", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingOutletItem }, new[] { importedItem });

            Assert.Equal(2, result.Count);
            Assert.Contains(result, item => OutletQuantity(item) == 3);
            Assert.Contains(result, item => OutletQuantity(item) == 0 && TotalQuantity(item) == 2);
        }

        [Fact]
        public void Merge_MergedLine_IsAlwaysUnpriced()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 0, 0, "ref", "notes", 8m, 16m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 3, 0, 0, "ref", "notes", 10m, 30m, "EUR", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Single(result);
            Assert.Equal(5, TotalQuantity(result[0]));
            Assert.Null(UnitPrice(result[0]));
            Assert.Null(Price(result[0]));
            Assert.Null(Currency(result[0]));
        }

        [Fact]
        public void Merge_MergedLineWithMismatchedCurrencies_PersistsUnpriced()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 0, 0, "ref", "notes", 10m, 20m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 3, 0, 0, "ref", "notes", 12m, 36m, "USD", new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Single(result);
            Assert.Null(UnitPrice(result[0]));
            Assert.Null(Price(result[0]));
            Assert.Null(Currency(result[0]));
        }

        [Fact]
        public void Merge_MergedLineWhereOnlyExistingIsPriced_IsUnpriced()
        {
            var productId = Guid.NewGuid();
            var existingItem = CreateItem(
                productId, 2, 0, 0, "ref", "notes", 10m, 20m, "EUR", new DateOnly(2026, 9, 1));
            var importedItem = CreateItem(
                productId, 3, 0, 0, "ref", "notes", null, null, null, new DateOnly(2026, 9, 1));

            var result = Merge(new[] { existingItem }, new[] { importedItem });

            Assert.Single(result);
            Assert.Equal(5, TotalQuantity(result[0]));
            Assert.Null(UnitPrice(result[0]));
            Assert.Null(Currency(result[0]));
            Assert.Null(Price(result[0]));
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

        [Fact]
        public void GroupImportedLines_SameSkuAndReferenceOnTwoRows_SumsIntoOneLine()
        {
            var lines = new[]
            {
                CreateOrderFileLine("SKU-1", 2, "ref", "notes"),
                CreateOrderFileLine("SKU-1", 3, "ref", "notes")
            };

            var result = GroupImportedLines(lines);

            Assert.Single(result);
            Assert.Equal("SKU-1", Sku(result[0]));
            Assert.Equal(5, Quantity(result[0]));
        }

        [Fact]
        public void GroupImportedLines_DifferentReferencesOnTwoRows_StaySeparate()
        {
            var lines = new[]
            {
                CreateOrderFileLine("SKU-1", 2, "ref-1", "notes"),
                CreateOrderFileLine("SKU-1", 3, "ref-2", "notes")
            };

            var result = GroupImportedLines(lines);

            Assert.Equal(2, result.Count);
            Assert.Equal(5, result.Sum(Quantity));
        }

        [Fact]
        public void GroupImportedLines_PreservesFirstSeenOrder()
        {
            var lines = new[]
            {
                CreateOrderFileLine("SKU-A", 1, "ref", "notes"),
                CreateOrderFileLine("SKU-B", 2, "ref", "notes"),
                CreateOrderFileLine("SKU-A", 3, "ref", "notes")
            };

            var result = GroupImportedLines(lines);

            Assert.Equal(2, result.Count);
            Assert.Equal("SKU-A", Sku(result[0]));
            Assert.Equal(4, Quantity(result[0]));
            Assert.Equal("SKU-B", Sku(result[1]));
            Assert.Equal(2, Quantity(result[1]));
        }

        [Fact]
        public void GroupImportedLines_NormalisesBlankReferenceToNull()
        {
            var lines = new[]
            {
                CreateOrderFileLine("SKU-1", 2, "  ", "notes")
            };

            var result = GroupImportedLines(lines);

            Assert.Single(result);
            Assert.Null(ExternalReference(result[0]));
        }
    }

    public sealed class BuyerOrderBasketUploadHelperTests : OrderBasketUploadHelperTests<BuyerBasketItem, BuyerProduct, BuyerOrderFileLine>
    {
        protected override BuyerBasketItem CreateItem(
            Guid productId, double quantity, double stockQuantity, double outletQuantity,
            string reference, string notes, decimal? unitPrice, decimal? price, string currency, DateOnly expectedLeadTime)
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

        protected override IReadOnlyList<BuyerBasketItem> Merge(IEnumerable<BuyerBasketItem> existingItems, IEnumerable<BuyerBasketItem> importedItems)
            => BuyerOrderBasketUploadHelper.Merge(existingItems, importedItems);

        protected override void DeductExistingStock(IDictionary<Guid, double> availableStockByProductId, IEnumerable<BuyerBasketItem> existingItems)
            => BuyerOrderBasketUploadHelper.DeductExistingStock(availableStockByProductId, existingItems);

        protected override BuyerProduct CreateProduct(string sku) => new() { Id = Guid.NewGuid(), Sku = sku };
        protected override IReadOnlyDictionary<string, BuyerProduct> CreateProductLookup(IEnumerable<BuyerProduct> products)
            => BuyerOrderBasketUploadHelper.CreateProductLookup(products);

        protected override Guid? ProductId(BuyerBasketItem item) => item.ProductId;
        protected override double TotalQuantity(BuyerBasketItem item) => item.Quantity + item.StockQuantity + item.OutletQuantity;
        protected override double OutletQuantity(BuyerBasketItem item) => item.OutletQuantity;
        protected override decimal? UnitPrice(BuyerBasketItem item) => item.UnitPrice;
        protected override decimal? Price(BuyerBasketItem item) => item.Price;
        protected override string Currency(BuyerBasketItem item) => item.Currency;

        protected override BuyerOrderFileLine CreateOrderFileLine(string sku, double quantity, string reference, string moreInfo)
            => new() { Sku = sku, Quantity = quantity, ExternalReference = reference, MoreInfo = moreInfo };

        protected override IReadOnlyList<BuyerOrderFileLine> GroupImportedLines(IEnumerable<BuyerOrderFileLine> lines)
            => BuyerOrderBasketUploadHelper.GroupImportedLines(lines);

        protected override string Sku(BuyerOrderFileLine line) => line.Sku;
        protected override double Quantity(BuyerOrderFileLine line) => line.Quantity;
        protected override string ExternalReference(BuyerOrderFileLine line) => line.ExternalReference;
    }

    public sealed class SellerOrderBasketUploadHelperTests : OrderBasketUploadHelperTests<SellerBasketItem, SellerProduct, SellerOrderFileLine>
    {
        protected override SellerBasketItem CreateItem(
            Guid productId, double quantity, double stockQuantity, double outletQuantity,
            string reference, string notes, decimal? unitPrice, decimal? price, string currency, DateOnly expectedLeadTime)
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

        protected override IReadOnlyList<SellerBasketItem> Merge(IEnumerable<SellerBasketItem> existingItems, IEnumerable<SellerBasketItem> importedItems)
            => SellerOrderBasketUploadHelper.Merge(existingItems, importedItems);

        protected override void DeductExistingStock(IDictionary<Guid, double> availableStockByProductId, IEnumerable<SellerBasketItem> existingItems)
            => SellerOrderBasketUploadHelper.DeductExistingStock(availableStockByProductId, existingItems);

        protected override SellerProduct CreateProduct(string sku) => new() { Id = Guid.NewGuid(), Sku = sku };
        protected override IReadOnlyDictionary<string, SellerProduct> CreateProductLookup(IEnumerable<SellerProduct> products)
            => SellerOrderBasketUploadHelper.CreateProductLookup(products);

        protected override Guid? ProductId(SellerBasketItem item) => item.ProductId;
        protected override double TotalQuantity(SellerBasketItem item) => item.Quantity + item.StockQuantity + item.OutletQuantity;
        protected override double OutletQuantity(SellerBasketItem item) => item.OutletQuantity;
        protected override decimal? UnitPrice(SellerBasketItem item) => item.UnitPrice;
        protected override decimal? Price(SellerBasketItem item) => item.Price;
        protected override string Currency(SellerBasketItem item) => item.Currency;

        protected override SellerOrderFileLine CreateOrderFileLine(string sku, double quantity, string reference, string moreInfo)
            => new() { Sku = sku, Quantity = quantity, ExternalReference = reference, MoreInfo = moreInfo };

        protected override IReadOnlyList<SellerOrderFileLine> GroupImportedLines(IEnumerable<SellerOrderFileLine> lines)
            => SellerOrderBasketUploadHelper.GroupImportedLines(lines);

        protected override string Sku(SellerOrderFileLine line) => line.Sku;
        protected override double Quantity(SellerOrderFileLine line) => line.Quantity;
        protected override string ExternalReference(SellerOrderFileLine line) => line.ExternalReference;
    }
}
