using Foundation.Extensions.Models;
using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class GetCountriesSalesServiceModel : BaseServiceModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
