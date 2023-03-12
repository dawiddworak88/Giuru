namespace Seller.Web.Areas.Dashboard.ApiRequestModels
{
    public class ProductsSalesAnalyticsApiRequestModel : SalesAnalyticsRequestModel
    {
        public string OrderBy { get; set; }
        public int? Size { get; set; }
    }
}
