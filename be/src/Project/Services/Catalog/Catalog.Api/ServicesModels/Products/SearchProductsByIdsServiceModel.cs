using System;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class SearchProductsByIdsServiceModel : SearchProductsServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
