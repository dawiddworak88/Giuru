namespace Buyer.Web.Shared.ViewModels.Analytics
{
    public class OrdersAnalyticsDetailViewModel
    {
        public string Title { get; set; }
        public string NameLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public SalesAnalyticsViewModel SalesAnalytics { get; set; }
    }
}
