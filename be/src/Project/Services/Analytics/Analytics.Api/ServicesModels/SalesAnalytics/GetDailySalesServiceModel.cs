using Analytics.Api.Shared.ServicesModels;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetDailySalesServiceModel : ChartBaseServiceModel
    {
        public bool IsSeller { get; set; }
    }
}
