using System;
using System.Collections.Generic;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class CreateSalesAnalyticsServiceModel
    {
        public Guid? ClientId { get; set; }
        public string Token { get; set; }
        public IEnumerable<CreateSalesAnalyticsProductServiceModel> Products { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
