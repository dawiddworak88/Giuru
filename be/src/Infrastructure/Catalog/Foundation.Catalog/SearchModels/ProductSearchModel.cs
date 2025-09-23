using Foundation.Search.SearchResultModels;
using Nest;
using System;
using System.Collections.Generic;

namespace Foundation.Catalog.SearchModels.Products
{
    [ElasticsearchType(RelationName = "product")]
    public class ProductSearchModel : SearchModelBase
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public CompletionField NameSuggest { get; set; }
        public string Description { get; set; }
        public string FormData { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public CompletionField CategoryNameSuggest { get; set; }
        public Guid SellerId { get; set; }
        public string BrandName { get; set; }
        public CompletionField BrandNameSuggest { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public string PrimaryProductSku { get; set; }
        public bool PrimaryProductIdHasValue { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public bool IsProtected { get; set; }
        public string Ean { get; set; }
        public int? FulfillmentTime { get; set; }
        public double? StockAvailableQuantity { get; set; }
        public double? OutletAvailableQuantity { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }

        [Nested]
        public Dictionary<string, object> ProductAttributes { get; set; }
    }
}
