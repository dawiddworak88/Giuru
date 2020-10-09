using Foundation.Search.SearchResultModels;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Products.SearchModels
{
    public class ProductSearchModel : SearchModelBase
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormData { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public Guid? PrimaryProductId { get; set; }
        public bool PrimaryProductIdHasValue { get; set; }
        public bool IsNew { get; set; }
        public bool IsProtected { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
