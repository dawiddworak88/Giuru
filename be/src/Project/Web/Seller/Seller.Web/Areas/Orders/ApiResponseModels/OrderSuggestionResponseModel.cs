using System.Collections.Generic;
using System;

namespace Seller.Web.Areas.Orders.ApiResponseModels
{
    public class OrderSuggestionResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public double StockQuantity { get; set; }
    }
}
