using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Dashboard.ViewModels
{
    public class ProductsSalesAnalyticsViewModel
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
        public string NameLabel { get; set; }
        public string SkuLabel { get; set; }
        public string QuantityLabel { get; set; }
        public IEnumerable<ProductSalesAnalyticsViewModel> Products { get; set; }
    }
}
