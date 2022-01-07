using Foundation.ApiExtensions.Models.Response;
using System;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductStockResponseModel : BaseResponseModel
    {
        public int? AvailableQuantity { get; set; }
        public int? RestockableInDays { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
    }
}
