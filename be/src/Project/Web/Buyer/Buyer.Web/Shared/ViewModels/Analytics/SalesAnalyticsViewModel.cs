using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Analytics
{
    public class SalesAnalyticsViewModel
    {
        public string Title { get; set; }
        public IEnumerable<string> ChartLables { get; set; }
        public IEnumerable<SalesAnalyticsChartDatasetsViewModel> ChartDatasets { get; set; }
    }
}
