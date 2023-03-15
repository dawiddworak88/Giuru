namespace Seller.Web.Areas.Dashboard.ViewModels
{
    public class DashboardDetailViewModel
    {
        public string Title { get; set; }
        public DailySalesAnalyticsViewModel DailySalesAnalytics { get; set; }
        public CountrySalesAnalyticsViewModel CountrySalesAnalytics { get; set; }
        public ClientsSalesAnalyticsViewModel ClientsSalesAnalytics { get; set; }
        public ProductsSalesAnalyticsViewModel ProductsSalesAnalytics { get; set; }
    }
}
