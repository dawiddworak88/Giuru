using Analytics.Api.Shared.ServicesModels;
using Foundation.Extensions.Models;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetTopSalesProductsAnalyticsServiceModel : ChartBaseServiceModel
    {
        public bool IsSeller { get; set; }
        public string OrderBy { get; set; }
        public int? Size { get; set; }
    }
}
