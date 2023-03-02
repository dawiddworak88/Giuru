using System;

namespace Seller.Web.Areas.Dashboard.RequestModels
{
    public class SalesAnalyticsRequestModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
