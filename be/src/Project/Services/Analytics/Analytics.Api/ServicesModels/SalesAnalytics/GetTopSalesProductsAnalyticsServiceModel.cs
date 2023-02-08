using Foundation.Extensions.Models;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetTopSalesProductsAnalyticsServiceModel : BaseServiceModel
    {
        public bool IsSeller { get; set; }
        public string OrderBy { get; set; }
        public int? Size { get; set; }
    }
}
