using Analytics.Api.Shared.ServicesModels;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetClientsSalesServiceModel : ChartBaseServiceModel
    {
        public string OrderBy { get; set; }
        public int? Size { get; set; }
    }
}
