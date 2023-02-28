using Foundation.Extensions.Models;
using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetAnnualSalesServiceModel : BaseServiceModel
    {
        public bool IsSeller { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
