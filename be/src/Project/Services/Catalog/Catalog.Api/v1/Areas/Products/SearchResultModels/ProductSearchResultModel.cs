using Foundation.Search.SearchResultModels;
using System;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Products.SearchResultModels
{
    public class ProductSearchResultModel : SearchResultModelBase
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormData { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<Guid> Videos { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
