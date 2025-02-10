using System.Collections.Generic;
using System;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductQuantitiesResponseModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
    }
}
