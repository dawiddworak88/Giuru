using System.Collections.Generic;

namespace Buyer.Web.Areas.Dashboard.ViewModels
{
    public class OrdersAnalyticsDetailViewModel
    {
        public string Title { get; set; }
        public string NumberOfOrdersLabel { get; set; }
        public string TopOrderedProducts { get; set; }
        public string NameLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public IEnumerable<string> ChartLables { get; set; }
        public IEnumerable<OrderAnalyticsChartDatasetsViewModel> ChartDatasets { get; set; }
        public IEnumerable<OrderAnalyticsProductViewModel> Products { get; set; }
    }
}
