using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductOrderSuggestionResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public double StockQuantity { get; set; }
    }
}
