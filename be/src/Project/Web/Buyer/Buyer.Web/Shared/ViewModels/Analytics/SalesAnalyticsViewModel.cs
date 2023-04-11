using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Analytics
{
    public class SalesAnalyticsViewModel
    {
        public string Title { get; set; }
        public string FromLabel { get; set; }
        public string ToLabel { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SaveUrl { get; set; }
        public string InvalidDateRangeErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string[] DatePickerViews { get; set; }
        public IEnumerable<string> ChartLabels { get; set; }
        public IEnumerable<SalesAnalyticsChartDatasetsViewModel> ChartDatasets { get; set; }
    }
}
