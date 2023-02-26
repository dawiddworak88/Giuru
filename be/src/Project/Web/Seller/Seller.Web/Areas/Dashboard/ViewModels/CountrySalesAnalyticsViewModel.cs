using Seller.Web.Shared.ViewModels;
using System;

namespace Seller.Web.Areas.Dashboard.ViewModels
{
    public class CountrySalesAnalyticsViewModel : ChartViewModel
    {
        public string FromLabel { get; set; }
        public string ToLabel { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SaveUrl { get; set; }
    }
}
