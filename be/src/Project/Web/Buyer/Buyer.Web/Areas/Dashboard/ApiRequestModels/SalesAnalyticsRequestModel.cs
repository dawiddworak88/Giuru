using System;

namespace Buyer.Web.Areas.Dashboard.ApiRequestModels
{
    public class SalesAnalyticsRequestModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
