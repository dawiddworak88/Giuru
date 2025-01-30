using Seller.Web.Areas.Products.DomainModels;

namespace Seller.Web.Areas.Products.ApiResponseModels
{
    public class ProductQuantitiesResponseModel : Product
    {
        public double StockQuantity { get; set; }
        public double OutletQuantity { get; set; }
    }
}
