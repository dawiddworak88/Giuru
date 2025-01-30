using Buyer.Web.Areas.Products.DomainModels;

namespace Buyer.Web.Areas.Products.ApiResponseModels
{
    public class ProductQuantitiesResponseModel : Product
    {
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
    }
}
