using Analytics.Api.Shared.ServicesModels;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetTopSalesProductsAnalyticsServiceModel : ChartBaseServiceModel
    {
        public bool IsSeller { get; set; }
        public string OrderBy { get; set; }
        public int? Size { get; set; }
    }
}
